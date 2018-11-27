using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DescriptorProcessor : IDescriptorProcessor
{
    private GameElementManager elementManager;

    private string defLang;

    private event EventHandler<EventArgs> OnReferenceProcessing;

    private ILogger logger;

    public DescriptorProcessor()
    {
        logger = Injector.Logger;
    }


    protected void ProcessGameDescriptor(GameDescriptor gameDescriptor)
    {
        ProcessGameProperties(gameDescriptor.GameProperties);
        ProcessTexts(gameDescriptor.Texts);
        ProcessPlayer(gameDescriptor.Player);
        ProcessItems(gameDescriptor.Items);
        ProcessProperties(gameDescriptor.Properties);
        ProcessMenuItems(gameDescriptor.MenuItems, gameDescriptor.gameName);
        ProcessRooms(gameDescriptor.Rooms);
        ProcessGameEnds(gameDescriptor.GameEnds);
        ProcessNpcs(gameDescriptor.NPCs);
    }

    public void ProcessMultipleGameDescriptor(List<GameDescriptor> gameDescriptors, Dictionary<string, MemoryStream> images)
    {
        if (gameDescriptors.Count == 0) return;
        
        GameDescriptor root = GetRootDescriptor(gameDescriptors);
        if (root == null) root = gameDescriptors[0];
        string gameName = root.gameName;
        string version = root.version ?? "";
        string gameStorageName = gameName + "_" + version;
        if (PersistanceHelper.CheckGameDirectory(gameStorageName))
        {
            SetExistingGemAsCurrent(gameStorageName);
            return;
        }

        elementManager = new GameElementManager(gameStorageName);
        PersistanceHelper.CreateStorage(gameStorageName);
        PersistanceHelper.StoreImages(gameStorageName, images);
        foreach (GameDescriptor descripor in gameDescriptors)
        {
            ProcessGameDescriptor(descripor);
        }
        if (OnReferenceProcessing != null)
        {
            OnReferenceProcessing(null, EventArgs.Empty);
        }
        elementManager.SetFirstRoom();
        PersistanceHelper.StoreInitialGEM(gameStorageName, elementManager);
        Injector.GameElementManager = elementManager;
    }

    public void SetExistingGemAsCurrent(string gameStorageName)
    {
        elementManager = PersistanceHelper.GetInitialGEM(gameStorageName);
        elementManager.savedGameNames = PersistanceHelper.GetSavedGameNames(gameStorageName);
        Injector.GameElementManager = elementManager;
    }

    private GameDescriptor GetRootDescriptor(List<GameDescriptor> gameDescriptors)
    {
        foreach (GameDescriptor descripor in gameDescriptors)
        {
            if (descripor.root)
            {
                return descripor;
            }
        }
        return null;
    }

    /**
     * Processes the texts to the GameElementManager.
     * Returns the processed GEText elements.
     */
    private SortedList<string, GEText> ProcessTexts(TextsType texts)
    {
        SortedList<string, GEText> processedTexts = new SortedList<string, GEText>();
        if(texts == null)
        {
            return processedTexts;
        }
        string currentDefLang = texts.defLang ?? defLang;
        foreach(TextsTypeText text in texts.Text)
        {
            processedTexts.Add(text.id, elementManager.AddText(text.id, text.Value, text.lang ?? currentDefLang));
        }
        return processedTexts;
    }

    private SortedList<string, GEProperty> ProcessProperties(PropertiesType properties)
    {
        SortedList<string, GEProperty> processedProperties = new SortedList<string, GEProperty>();
        if(properties == null)
        {
            return processedProperties;
        }
        foreach(PropertiesTypeProperty property in properties.Property)
        {
            GEProperty newProperty = new GEProperty(property.id, null, property.defValue, property.minValue, property.maxValue);
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newProperty.Name = elementManager.GetTextElement(property.nameTextId);
            };
            elementManager.AddProperty(newProperty);
            processedProperties.Add(property.id, newProperty);
        }

        return processedProperties;
    }

    private SortedList<string, GEProperty> ProcessProperties(PropertiesWithRefType properties)
    {
        SortedList<string, GEProperty> processedProperties = ProcessProperties((PropertiesType)properties);
        if (properties != null)
        {
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                ProcessRefs(properties.PropertyRef, processedProperties, elementManager.GetProperty);
            };
        }
        return processedProperties;
    }

    private SortedList<string, GEItem> ProcessItems(ItemsType items)
    {
        SortedList<string, GEItem> processedItems = new SortedList<string, GEItem>();
        if (items == null) return processedItems;
        foreach(ItemsTypeItem item in items.Item)
        {
            SortedList<string, GEMenuItem> menuItems = ProcessMenuItems(item.MenuItems, item.id);
            SortedList<string, GEProperty> properties = ProcessProperties(item.Properties);
            SortedList<string, GEText> texts = ProcessTexts(item.Texts);
            GEItem newItem = new GEItem(item.id, null, item.activeAtStart, item.equipable, null)
            {
                MenuItems = menuItems,
                Properties = properties,
                Texts = texts
            };
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newItem.ItemName = elementManager.GetTextElement(item.nameTextId);
                newItem.Description = elementManager.GetTextElement(item.descTextId);
            };
            elementManager.AddItem(newItem);
            processedItems.Add(item.id, newItem);
        }
        return processedItems;
    }

    private SortedList<string, GEItem> ProcessItems(ItemsWithRefsType items)
    {
        SortedList<string, GEItem> processedItems = ProcessItems((ItemsType)items);
        if (items != null)
        {
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                ProcessRefs(items.ItemRef, processedItems, elementManager.GetItem);
            };
        }
        return processedItems;
    }

    private SortedList<string, GEMenuItem> ProcessMenuItems(MenuItemsType menuItems, string parentId)
    {
        SortedList<string, GEMenuItem> processedMenuItems = new SortedList<string, GEMenuItem>();
        if (menuItems == null) return processedMenuItems;
        foreach(MenuItemsTypeMenuItem menuItem in menuItems.MenuItem)
        {
            SortedList<string, GEText> texts = ProcessTexts(menuItem.Texts);
            List<GEAction> actionsToAdd = new List<GEAction>();
            foreach (ActionsType action in menuItem.Actions)
            {
                actionsToAdd.Add(ProcessActions(action, parentId));
            }
            GERequirement requirements = ProcessRequirements(menuItem.Requirements);
            GEMenuItem newMenuItem = new GEMenuItem(menuItem.id, null, actionsToAdd, requirements, texts, menuItem.activeAtStart, menuItem.useNumber);
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newMenuItem.MenuName = elementManager.GetTextElement(menuItem.menuTextId);
            };
            elementManager.AddMenuItem(newMenuItem);
            processedMenuItems.Add(menuItem.id, newMenuItem);
        }
        return processedMenuItems;
    }

    private SortedList<string, GEMenuItem> ProcessMenuItems(MenuItemsWithRefsType menuItems, string parentId)
    {
        SortedList<string, GEMenuItem> processedMItems = ProcessMenuItems((MenuItemsType)menuItems, parentId);
        if (menuItems != null)
        {
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                ProcessRefs(menuItems.MenuItemRef, processedMItems, elementManager.GetMenuItem);
            };
        }
        return processedMItems;
    }

    private GERequirement ProcessRequirements(RequirementsType requirements)
    {
        if (requirements == null) return null;
        List<GERequirement.GEActivationChecker> activationCheckers = new List<GERequirement.GEActivationChecker>();
        foreach(RequirementsTypeIsActive aChecker in requirements.isActive)
        {
            activationCheckers.Add(new GERequirement.GEActivationChecker(elementManager, aChecker.refId, aChecker.value));
        }
        List<GERequirement.GEPropertyChecker> propertyCheckers = new List<GERequirement.GEPropertyChecker>();
        foreach (RequirementsTypePropertyCondition pChecker in requirements.propertyCondition)
        {
            PropertyConditionEnum condition = pChecker.condition;
            GERequirement.PropertyConditionType cType = GERequirement.PropertyConditionType.EQ;
            if (!condition.Equals(PropertyConditionEnum.eq))
            {
                cType = condition.Equals(PropertyConditionEnum.lt) ? GERequirement.PropertyConditionType.LT : GERequirement.PropertyConditionType.GT;
            }

            propertyCheckers.Add(new GERequirement.GEPropertyChecker(elementManager, pChecker.refId, cType, pChecker.value));
        }
        List<GERequirement.GEEquippedChecker> equippedCheckers = new List<GERequirement.GEEquippedChecker>();
        foreach (RequirementsTypeIsEquipped aChecker in requirements.isEquipped)
        {
            equippedCheckers.Add(new GERequirement.GEEquippedChecker(elementManager, aChecker.refId, aChecker.value));
        }
        GERequirement requirement = new GERequirement(null,activationCheckers, propertyCheckers, equippedCheckers);
        OnReferenceProcessing += delegate (object o, EventArgs e)
        {
            requirement.TextOnFail = elementManager.GetTextElement(requirements.textOnFail);
        };
        return requirement;
    }

    private GEAction ProcessActions(ActionsType actions, string parentId)
    {
        List<GEAction.GEActivation> activations = new List<GEAction.GEActivation>();
        foreach(ActionsTypeSetActive activation in actions.setActive)
        {
            ActivationSetEnum vEnum = activation.value;
            bool? value = null;
            if (vEnum != ActivationSetEnum.@switch)
            {
                value = vEnum == ActivationSetEnum.@true ? true : false;
            }
            activations.Add(new GEAction.GEActivation(activation.refId, value));
        }
        List<GEAction.GEPropertySetter> propSetter = new List<GEAction.GEPropertySetter>();
        foreach (ActionsTypeSetPropertyValue prop in actions.setPropertyValue)
        {
            GEAction.PropertyChangeType propEnum = GEAction.PropertyChangeType.SET;
            PropertyChangeEnum change = prop.change;
            if (!change.Equals(PropertyChangeEnum.set))
            {
                propEnum = change.Equals(PropertyChangeEnum.inc) ? GEAction.PropertyChangeType.SET : GEAction.PropertyChangeType.DEC;
            }

            propSetter.Add(new GEAction.GEPropertySetter(prop.refId, propEnum, prop.value));
        }
        GEAction.GESaveAction saveAction = null;
        if(actions.SaveGame != null)
        {
            saveAction = new GEAction.GESaveAction(actions.SaveGame.isAutoSave, parentId);
        }
        GEAction processedAction = new GEAction(elementManager, null, activations, propSetter, saveAction, actions.onUseIntervalTo);
        OnReferenceProcessing += delegate (object o, EventArgs e)
        {
            processedAction.ResponseText = elementManager.GetTextElement(actions.responseTextId);
        };
        return processedAction;
    }

    private GENpc.GEItemAction ProcessActions(ItemActionsType action, string parentId)
    {
        GEAction actionBase = ProcessActions((ActionsType)action, parentId);

        List<GENpc.GEItemAction.GEEquipItem> equipItems = new List<GENpc.GEItemAction.GEEquipItem>();
        foreach (ItemActionsTypeEquipItem equipItem in action.equipItem)
        {
            equipItems.Add(new GENpc.GEItemAction.GEEquipItem(equipItem.refId, equipItem.value));
        }
        GENpc.GEItemAction newAction = new GENpc.GEItemAction(elementManager, null, actionBase.Activations, actionBase.PropertySetters, actionBase.SaveAction, equipItems, actionBase.UseInterval);
        OnReferenceProcessing += delegate (object o, EventArgs e)
        {
            newAction.ResponseText = elementManager.GetTextElement(action.responseTextId);
        };
        return newAction;
    }

    private void ProcessPlayer(PlayerType player)
    {
        if (player == null) return;
        elementManager.Player = new GEPlayer(ProcessProperties(player.Properties), ProcessItems(player.Items));
    }

    private void ProcessGameProperties(GamePropertiesType gameProperties)
    {
        if (gameProperties == null) return;
        elementManager.GameProperties = new GEGameProperties(gameProperties.firstRoomId, gameProperties.defaultLang, null, gameProperties.menuImgSrc, null, gameProperties.menuSaveAvailable, gameProperties.checkpointsOn);
        elementManager.DefLang = gameProperties.defaultLang;
        this.defLang = gameProperties.defaultLang;
        OnReferenceProcessing += delegate (object o, EventArgs e)
        {
            elementManager.GameProperties.GameNameText = elementManager.GetTextElement(gameProperties.gameNameTextId);
            elementManager.GameProperties.GreetingText = elementManager.GetTextElement(gameProperties.greetingTextId);
        };
    }

    private void ProcessRooms(RoomsType rooms)
    {
        if (rooms == null) return;
        foreach (RoomsTypeRoom room in rooms.Room)
        {
            GERoom newRoom = new GERoom(room.id, null, room.imgSrc, null, room.activeAtStart, room.isCheckpoint)
            {
                Properties = ProcessProperties(room.Properties),
                MenuItems = ProcessMenuItems(room.MenuItems, room.id),
                Items = ProcessItems(room.Items),
                Npcs = ProcessNpcs(room.NPCs),
                Texts = ProcessTexts(room.Texts),
                Neighbours = ProcessNeighbours(room.Neighbours)
            };

            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newRoom.NameText = elementManager.GetTextElement(room.nameTextId);
                if(room.descTextId != null)
                {
                    newRoom.DescText = elementManager.GetTextElement(room.descTextId);
                } else
                {
                    logger.LogInfo("There was no descTextId given for the Room with id: " + room.id);
                }
            };

            elementManager.AddRoom(newRoom);
        }
    }

    private SortedList<string, GENeighbour> ProcessNeighbours(NeighboursType neighbours)
    {
        SortedList<string, GENeighbour> processedNeighbours = new SortedList<string, GENeighbour>();
        if (neighbours == null) return processedNeighbours;
        foreach(NeighboursTypeNeighbour neighbour in neighbours.Neighbour)
        {
            GERequirement requirements = ProcessRequirements(neighbour.Requirements);
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                GENeighbour newNeighbour = new GENeighbour(neighbour.id, neighbour.activeAtStart)
                {
                    MenuText = elementManager.GetTextElement(neighbour.nameTextId),
                    Room = elementManager.GetRoom(neighbour.roomRefId),
                    Requirements = requirements
                };
                processedNeighbours.Add(neighbour.id, newNeighbour);
                elementManager.AddNeighbour(newNeighbour);
            };
        }
        return processedNeighbours;
    }

    private SortedList<string, GENpc> ProcessNpcs(NPCsType npcs)
    {
        SortedList<string, GENpc> processedNpcs = new SortedList<string, GENpc>();
        if (npcs == null) return processedNpcs;
        foreach (NPCsTypeNPC npc in npcs.NPC)
        {
            SortedList<string, GEText> texts = ProcessTexts(npc.Texts);
            SortedList<string, GEItem> items = ProcessItems(npc.Items);
            SortedList<string, GEMenuItem> menuItems = ProcessMenuItems(npc.MenuItems, npc.id);

            GENpc newNpc = new GENpc(npc.id, npc.activeAtStart)
            {
                Items = items,
                Properties = ProcessProperties(npc.Properties),
                Texts = texts,
                MenuItems = menuItems
            };
            SortedList<string, GENpc.GEConversation> conversations = ProcessConversations(npc.Conversation, newNpc);
            newNpc.Conversations = conversations;

            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newNpc.NameText = elementManager.GetTextElement(npc.nameTextId);
                if (npc.descTextId != null)
                {
                    newNpc.DescText = elementManager.GetTextElement(npc.descTextId);
                }
                else
                {
                    logger.LogInfo("There was no descTextId given for the Room with id: " + npc.id);
                }
            };
            processedNpcs.Add(npc.id, newNpc);
            elementManager.AddNPC(newNpc);
        }
        return processedNpcs;
    }

    private SortedList<string, GENpc> ProcessNpcs(NPCsWithRefType npcs)
    {
        SortedList<string, GENpc> processedNpcs = ProcessNpcs((NPCsType)npcs);
        if (npcs != null)
        {
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                ProcessRefs(npcs.NPCRef, processedNpcs, elementManager.GetNPC);
            };
        }
        return processedNpcs;
    }

    private SortedList<string, GENpc.GEConversation> ProcessConversations(List<NPCsTypeNPCConversation> conversations, GENpc npc)
    {
        SortedList<string, GENpc.GEConversation> processedConvs = new SortedList<string, GENpc.GEConversation>();
        if (conversations == null || conversations.Count == 0) return processedConvs;

        foreach(NPCsTypeNPCConversation conv in conversations)
        {
            GENpc.GEConversation newConv = new GENpc.GEConversation(conv.id, conv.activeAtStart, npc);
            SortedList<string, GENpc.GELine> lines = ProcessLines(conv.Line, newConv);
            SortedList<string, GEText> texts = ProcessTexts(conv.Texts);
            newConv.FirstLine = lines[conv.firstLineId];
            newConv.Lines = lines;
            newConv.Texts = texts;
            processedConvs.Add(conv.id, newConv);
            elementManager.AddNpcConv(newConv);
        }

        return processedConvs;
    }

    private SortedList<string, GENpc.GELine> ProcessLines(List<NPCsTypeNPCConversationLine> lines, GENpc.GEConversation conv)
    {
        SortedList<string, GENpc.GELine> processedLines = new SortedList<string, GENpc.GELine>();
        if (lines == null || lines.Count == 0) return processedLines;

        foreach (NPCsTypeNPCConversationLine line in lines)
        {
            GENpc.GELine newLine = new GENpc.GELine(line.id, conv, line.isLastLine);
            List<GENpc.GEAnswer> answers = ProcessAnswers(line.Answer, newLine);
            newLine.Answers = answers;
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newLine.LineText = elementManager.GetTextElement(line.lineTextId);
            };
            processedLines.Add(line.id, newLine);
            elementManager.AddNpcConvLine(newLine);
        }

        return processedLines;
    }

    private List<GENpc.GEAnswer> ProcessAnswers(List<NPCsTypeNPCConversationLineAnswer> answers, GENpc.GELine line)
    {
        List<GENpc.GEAnswer> processedAnswers = new List<GENpc.GEAnswer>();
        if (answers == null || answers.Count == 0) return processedAnswers;

        foreach (NPCsTypeNPCConversationLineAnswer answer in answers)
        {
            GERequirement requirement = ProcessRequirements(answer.Requirements);
            GENpc.GEItemAction action = ProcessActions(answer.Actions, answer.id);

            GENpc.GEAnswer newAnser = new GENpc.GEAnswer(answer.id, answer.activeAtStart, line)
            {
                Requirement = requirement,
                Action = action
            };
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newAnser.AnswerText = elementManager.GetTextElement(answer.textId);
                newAnser.NextLine = elementManager.GetNpcConvLine(answer.nextLineId);
            };
            processedAnswers.Add(newAnser);
        }

        return processedAnswers;
    }

    private void ProcessGameEnds(GameEndsType gameEnds)
    {
        if (gameEnds == null) return;
        foreach (GameEndsTypeGameEnd gameEnd in gameEnds.GameEnd)
        {
            GEGameEnd newGameEnd = new GEGameEnd(gameEnd.id);
            OnReferenceProcessing += delegate (object o, EventArgs e)
            {
                newGameEnd.EndText = elementManager.GetTextElement(gameEnd.textId);
                newGameEnd.GameOverText = elementManager.GetTextElement(gameEnd.gameOverTextId);
            };
            elementManager.AddGameEnd(newGameEnd);
        }
    }

    private void ProcessRefs<T>(List<ReferenceType> refs, SortedList<string, T> destDic, Func<string, T> elementGetter) where T : GameElement
    {
        if (refs == null || refs.Count == 0) return;
        foreach (ReferenceType itemRef in refs)
        {
            T referredItem = elementGetter(itemRef.refId);
            if (referredItem == null)
            {
                logger.LogWarn("There is no element with the given id! " + itemRef.refId);
            }
            else
            {
                if (destDic.ContainsKey(referredItem.Id))
                {
                    logger.LogWarn("The referred game element is already processed with the given refId! redId: " + itemRef.refId);
                }
                else
                {
                    destDic.Add(referredItem.Id, referredItem);
                }
            }
        }
    }
    
}
