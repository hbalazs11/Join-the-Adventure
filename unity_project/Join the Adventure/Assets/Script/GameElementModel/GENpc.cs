using System;
using System.Collections.Generic;

[Serializable]
public class GENpc : ActivatableGameElement
{
    private GEConversation activeConversation;

    private GEText nameText;
    private GEText descText;

    SortedList<string, GEText> texts;
    SortedList<string, GEItem> items;
    SortedList<string, GEMenuItem> menuItems;
    SortedList<string, GEConversation> conversations;

    public GENpc(string id, bool isActive) : base(id)
    {
        this.isActive = isActive;
    }

    public GEConversation ActiveConversation
    {
        get
        {
            return activeConversation;
        }
    }

    public GEText NameText
    {
        get
        {
            return nameText;
        }

        set
        {
            nameText = value;
        }
    }

    public GEText DescText
    {
        get
        {
            return descText;
        }

        set
        {
            descText = value;
        }
    }

    public SortedList<string, GEText> Texts
    {
        get
        {
            return texts;
        }

        set
        {
            texts = value;
        }
    }

    public SortedList<string, GEItem> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public SortedList<string, GEConversation> Conversations
    {
        get
        {
            return conversations;
        }

        set
        {
            conversations = value;
            foreach(GEConversation conv in conversations.Values)
            {
                if (conv.IsActive())
                {
                    activeConversation = conv;
                    break;
                }
            }
        }
    }

    public SortedList<string, GEMenuItem> MenuItems
    {
        get
        {
            return menuItems;
        }

        set
        {
            menuItems = value;
        }
    }

    [Serializable]
    public class GEConversation : ActivatableGameElement
    {

        private GENpc parentNPC;
        private GELine firstLine;

        private SortedList<string, GELine> lines;
        private SortedList<string, GEText> texts;
        private SortedList<string, GERequirement> requirements;


        public GEConversation(string id, bool isActive, GENpc parent) : base(id)
        {
            this.isActive = isActive;
            this.parentNPC = parent;
        }

        public SortedList<string, GELine> Lines
        {
            get
            {
                return lines;
            }

            set
            {
                lines = value;
            }
        }

        public GELine FirstLine
        {
            get
            {
                return firstLine;
            }

            set
            {
                firstLine = value;
            }
        }

        public SortedList<string, GEText> Texts
        {
            get
            {
                return texts;
            }

            set
            {
                texts = value;
            }
        }

        public SortedList<string, GERequirement> Requirements
        {
            get
            {
                return requirements;
            }

            set
            {
                requirements = value;
            }
        }

        private void OnActivation()
        {
            if (isActive)
            {
                this.parentNPC.activeConversation = this;
            }
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            OnActivation();
        }

    }

    [Serializable]
    public class GELine : GameElement
    {

        private GEConversation parentConv;
        private GELine nextLine;
        private GEText lineText;
        List<GEAnswer> answers;
        private bool isLastLine;
        
        public GELine(string id, GEConversation parent, bool isLastLine) : base(id)
        {
            this.parentConv = parent;
            this.isLastLine = isLastLine;
        }

        public GELine NextLine
        {
            get
            {
                return nextLine;
            }

            set
            {
                nextLine = value;
            }
        }

        public GEText LineText
        {
            get
            {
                return lineText;
            }

            set
            {
                lineText = value;
            }
        }

        public List<GEAnswer> Answers
        {
            get
            {
                return answers;
            }

            set
            {
                answers = value;
            }
        }

        public GEConversation ParentConv
        {
            get
            {
                return parentConv;
            }

            set
            {
                parentConv = value;
            }
        }

        public bool IsLastLine
        {
            get
            {
                return isLastLine;
            }

            set
            {
                isLastLine = value;
            }
        }
    }

    [Serializable]
    public class GEAnswer : ActivatableGameElement
    {
        private GELine parentLine;

        private GELine nextLine;
        private GEText answerText;
        GERequirement requirement;
        GEItemAction action;

        public GEAnswer(string id, bool isActive, GELine parent) : base(id)
        {
            this.parentLine = parent;
            this.isActive = isActive; 
        }

        public string Execute()
        {
            if (requirement != null && !requirement.Check())
            {
                return requirement.TextOnFail.GetText();
            }
            if (action != null)
            {
                return action.Execute().GetText();
            }
            return null;
        }

        public GELine NextLine
        {
            get
            {
                return nextLine;
            }

            set
            {
                nextLine = value;
            }
        }

        public GEText AnswerText
        {
            get
            {
                return answerText;
            }

            set
            {
                answerText = value;
            }
        }

        public GELine ParentLine
        {
            get
            {
                return parentLine;
            }

            set
            {
                parentLine = value;
            }
        }

        public GERequirement Requirement
        {
            get
            {
                return requirement;
            }

            set
            {
                requirement = value;
            }
        }

        public GEItemAction Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }
    }

    [Serializable]
    public class GEItemAction : GEAction
    {
        List<GEEquipItem> equipItems;

        public GEItemAction(GameElementManager elementManager, GEText responseText, List<GEActivation> activations, List<GEPropertySetter> propertySetters, List<GEEquipItem> equipItems, int useInterval) : base(elementManager, responseText, activations, propertySetters, useInterval)
        {
            this.equipItems = equipItems;
        }
        
        public new GEText Execute()
        {
            foreach(GEEquipItem equipItem in equipItems)
            {
                equipItem.Execute(elementManager);
            }
            GEText response = base.Execute();
            return response;
        }

        [Serializable]
        public class GEEquipItem
        {
            private string refId;
            private bool isForEquip;
            public GEEquipItem(string refId, bool toEquip)
            {
                this.refId = refId;
                this.isForEquip = toEquip;
            }

            public void Execute(GameElementManager elementManager)
            {
                GEItem item = elementManager.GetItem(refId);
                if (isForEquip)
                {
                    item.Equip(elementManager);
                } else
                {
                    item.Unequip(elementManager);
                }

            }
        }
    }
}