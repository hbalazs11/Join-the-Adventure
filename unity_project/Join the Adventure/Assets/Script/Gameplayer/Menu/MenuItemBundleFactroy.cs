﻿using System;
using System.Collections.Generic;

public class MenuItemBundleFactroy  {
    
	public static MenuItemBundle CreateBundle(GERoom room, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(room.NameText.GetText(), parent);
        //Action menuitems
        foreach(GEMenuItem action in room.MenuItems.Values)
        {
            newBundle.AddMenuItem(new MIGameElementAction(action.MenuName.GetText(), newBundle, action));
            action.OnActivationChange += newBundle.RefreshOnEvent;
        }

        //Items menuitem
        newBundle.AddMenuItem(MIOpenList<GEItem>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.ITEMS), newBundle, room.Items.Values));
        
        //NPC menuitem
        newBundle.AddMenuItem(MIOpenList<GENpc>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.NPCS), newBundle, room.Npcs.Values));

        //Props menuItem
        List<GEProperty> propsToShow = GEProperty.GetPropertiesWithNames(room.Properties.Values);
        if (propsToShow.Count != 0)
        {
            newBundle.AddMenuItem(new MIShowDescription(LabelUtility.Instance.GetLabel(LabelNames.ROOMPROPS), newBundle, () => GEProperty.GetPropertyDescText(propsToShow)));
        }

        //Back menuitem
        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateBundle(GEItem item, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(item.ItemName.GetText(), parent);
        if (item.Equipable && !item.IsEquipped)
        {
            newBundle.AddMenuItem(new MIEquip(item, newBundle));
        }
        foreach (GEMenuItem action in item.MenuItems.Values)
        {
            newBundle.AddMenuItem(new MIGameElementAction(action.MenuName.GetText(), newBundle, action));
            action.OnActivationChange += newBundle.RefreshOnEvent;
        }
        //Props menuItem
        List<GEProperty> propsToShow = GEProperty.GetPropertiesWithNames(item.Properties.Values);
        if (propsToShow.Count != 0)
        {
            newBundle.AddMenuItem(new MIShowDescription(LabelUtility.Instance.GetLabel(LabelNames.ITEMPROPS), newBundle, () => GEProperty.GetPropertyDescText(propsToShow)));
        }

        newBundle.AddMenuItem(new MIBack(parent));
        Description.GetInstance().AddDescriptionText(item.Description.GetText());
        return newBundle;
    }
    

    public static MenuItemBundle CreateListBundle(string bundleName, List<GEItem> geItems, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        if(!ActivatableGameElement.IsActiveGEInList(geItems))
        {
            Description.GetInstance().AddDescriptionText(LabelUtility.Instance.GetLabel(LabelNames.EMPTYITEMLIST));
        }
        foreach (GEItem item in geItems)
        {
            newBundle.AddMenuItem(new MIOpenBundle<GEItem>(item.ItemName.GetText(), item, newBundle));
            item.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateListBundle(string bundleName, List<GENeighbour> geNeighbours, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        if (!ActivatableGameElement.IsActiveGEInList(geNeighbours))
        {
            Description.GetInstance().AddDescriptionText(LabelUtility.Instance.GetLabel(LabelNames.EMPTYNEIGHBOURLIST));
        }
        foreach (GENeighbour neighbour in geNeighbours)
        {
            newBundle.AddMenuItem(new MIRoomChange(neighbour.MenuText.GetText(), newBundle, neighbour));
            neighbour.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    //Creates the menuitem set that is shown for an NPC
    public static MenuItemBundle CreateBundle(GENpc npc, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(npc.NameText.GetText(), parent);

        foreach (GEMenuItem action in npc.MenuItems.Values)
        {
            newBundle.AddMenuItem(new MIGameElementAction(action.MenuName.GetText(), newBundle, action));
            action.OnActivationChange += newBundle.RefreshOnEvent;
        }
        //Props menuItem
        List<GEProperty> propsToShow = GEProperty.GetPropertiesWithNames(npc.Properties.Values);
        if (propsToShow.Count != 0)
        {
            newBundle.AddMenuItem(new MIShowDescription(LabelUtility.Instance.GetLabel(LabelNames.NPCPROPS), newBundle, () => GEProperty.GetPropertyDescText(propsToShow)));
        }

        //newBundle.AddMenuItem(new MIShowDescription(LabelUtility.Instance.GetLabel(LabelNames.SHOWDESCRUPTION), newBundle, npc.DescText.GetText()));

        //FIXME: this runs only once, when the bundle is created. Feature or bug? We might not want it to see more thane once tho...
        Description.GetInstance().AddDescriptionText(npc.DescText.GetText()); 

        
        newBundle.AddMenuItem(new MIConversation(LabelUtility.Instance.GetLabel(LabelNames.STARTCONVERSATION), parent, npc));


        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateListBundle(string bundleName, List<GENpc> geNpcs, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        if (!ActivatableGameElement.IsActiveGEInList(geNpcs))
        {
            Description.GetInstance().AddDescriptionText(LabelUtility.Instance.GetLabel(LabelNames.EMPTYNPCLIST));
        }
        foreach (GENpc npc in geNpcs)
        {
            newBundle.AddMenuItem(new MIOpenBundle<GENpc>(npc.NameText.GetText(), npc, newBundle));
            npc.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateConversationLineBundle(string bundleName, MenuItemBundle parent, GENpc.GELine line, string talkerName)
    {
        Description descriptionPanel = Description.GetInstance();
        
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        newBundle.OnExecutionSidefects += delegate (object o, EventArgs e)
        {
            descriptionPanel.AddDescriptionText(talkerName + ":\n" + line.LineText.GetText());
        };
        
        if(line.Answers.Count == 0 || line.IsLastLine)
        {
            newBundle.AddMenuItem(new MIBack(parent));
            return newBundle;
        }

        int i = 1;
        bool isShort = GENpc.GEAnswer.IsAnswerShort(line.Answers, 12);
        foreach (GENpc.GEAnswer answer in line.Answers)
        {
            if (isShort)
            {
                newBundle.AddMenuItem(new MIAnswer(answer.AnswerText.GetText(), answer, parent, talkerName));
            }
            else
            {
                MIAnswer item = new MIAnswer(i++.ToString(), answer, parent, talkerName);
                newBundle.AddMenuItem(item);
                newBundle.OnExecutionSidefects += delegate (object o, EventArgs e)
                {
                    if(answer.IsActive())
                        descriptionPanel.AddDescriptionText(item.GetMenuText() + ": " + answer.AnswerText.GetText());
                };
            }
            answer.OnActivationChange += newBundle.RefreshOnEvent;
        }
        return newBundle;
    }
}
