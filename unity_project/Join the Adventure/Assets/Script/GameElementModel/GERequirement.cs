using System.Collections.Generic;

public class GERequirement
{
    private GEText textOnFail;

    private List<GEActivationChecker> activationChecks;
    private List<GEPropertyChecker> propertyCehecks;
    private List<GEEquippedChecker> equippedChecks;

    public GERequirement(GEText textOnFail, List<GEActivationChecker> activationChecks, List<GEPropertyChecker> propertyCehecks, List<GEEquippedChecker> equippedChecks)
    {
        this.textOnFail = textOnFail;
        this.activationChecks = activationChecks;
        this.propertyCehecks = propertyCehecks;
        this.equippedChecks = equippedChecks;
    }

    public bool Check()
    {
        if(activationChecks != null)
        {
            foreach(GEActivationChecker checker in activationChecks)
            {
                if (!checker.Check()) return false;
            }
        }
        if (propertyCehecks != null)
        {
            foreach (GEPropertyChecker checker in propertyCehecks)
            {
                if (!checker.Check()) return false;
            }
        }
        if (equippedChecks != null)
        {
            foreach (GEEquippedChecker checker in equippedChecks)
            {
                if (!checker.Check()) return false;
            }
        }
        return true;
    }

    public GEText TextOnFail
    {
        get
        {
            return textOnFail;
        }
        set
        {
            textOnFail = value;
        }
    }

    public class GEActivationChecker
    {
        private string refId;
        private bool valueToCheck;
        private GameElementManager elementManager;

        public GEActivationChecker(GameElementManager elementManager, string refId, bool valueToCheck)
        {
            this.elementManager = elementManager;
            this.refId = refId;
            this.valueToCheck = valueToCheck;
        }

        public bool Check()
        {
            IActivatable element = elementManager.GetActivatableGameElement(refId);
            if(element == null)
            {
                System.Console.WriteLine("There is no activatable element with the given id! " + refId);
                return false;
            }
            return element.IsActive() == valueToCheck;
        }
    }

    public class GEPropertyChecker
    {
        private string refId;
        private PropertyConditionType conditionType;
        private double valueToCheck;
        private GameElementManager elementManager;

        public GEPropertyChecker(GameElementManager elementManager, string refId, PropertyConditionType conditionType, double valueToCheck)
        {
            this.elementManager = elementManager;
            this.refId = refId;
            this.conditionType = conditionType;
            this.valueToCheck = valueToCheck;
        }

        public bool Check()
        {
            GEProperty propertyToCheck = elementManager.GetProperty(refId);
            if (propertyToCheck == null)
            {
                System.Console.WriteLine("There is no property element with the given id! " + refId);
                return false;
            }
            switch (conditionType)
            {
                case PropertyConditionType.EQ: return propertyToCheck.Value == valueToCheck;
                case PropertyConditionType.LT: return propertyToCheck.Value < valueToCheck;
                case PropertyConditionType.GT: return propertyToCheck.Value > valueToCheck;
                default: return false;
            }

        }
    }

    public enum PropertyConditionType
    {
        EQ, LT, GT
    }

    public class GEEquippedChecker
    {
        private string refId;
        private bool valueToCheck;
        private GameElementManager elementManager;

        public GEEquippedChecker(GameElementManager elementManager, string refId, bool valueToCheck = true)
        {
            this.elementManager = elementManager;
            this.refId = refId;
            this.valueToCheck = valueToCheck;
        }

        public bool Check()
        {
            GEItem item = elementManager.GetGameElement(refId) as GEItem;
            
            if (item == null)
            {
                System.Console.WriteLine("There is no item with the given id! " + refId);
                return false;
            }
            return item.IsEquipped == valueToCheck;
        }
    }
}