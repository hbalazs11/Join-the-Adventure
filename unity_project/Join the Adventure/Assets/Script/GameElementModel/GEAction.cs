using System;
using System.Collections.Generic;

[Serializable]
public class GEAction
{
    private GEText responseText;
    private int useInterval; 

    List<GEActivation> activations;
    List<GEPropertySetter> propertySetters;
    GESaveAction saveAction;

    protected GameElementManager elementManager;

    public GEAction(GameElementManager elementManager, GEText responseText, List<GEActivation> activations, List<GEPropertySetter> propertySetters, GESaveAction saveAction, int useInterval)
    {
        this.elementManager = elementManager;
        this.responseText = responseText;
        this.activations = activations;
        this.propertySetters = propertySetters;
        this.useInterval = useInterval;
        this.saveAction = saveAction;
    }

    public GEText Execute()
    {
        foreach(GEActivation activation in activations)
        {
            activation.Execute(elementManager);
        }

        foreach (GEPropertySetter propSetter in propertySetters)
        {
            propSetter.Execute(elementManager);
        }
        if (saveAction != null)
        {
            saveAction.Execute();
        }
        return responseText;
    }


    [Serializable]
    public class GEActivation 
    {

        private string refId;
        private bool? valueToSet; // if null -> switch
        public GEActivation(string refId, bool? valueToSet) 
        {
            this.refId = refId;
            this.valueToSet = valueToSet;
        }

        public void Execute(GameElementManager elementManager)
        {
            IActivatable activatable = elementManager.GetActivatableGameElement(refId);
            if(activatable == null)
            {
                Injector.Logger.LogWarn("There is no activatable element with the given id, however an action is reffering it! The given id: " + refId);
            }
            if(valueToSet == null)
            {
                activatable.SetActive(!activatable.IsActive());
            } else
            {
                activatable.SetActive((bool) valueToSet);
            }

        }
    }
    [Serializable]
    public class GEPropertySetter
    {
        private string refId;
        PropertyChangeType changeType;
        double value;

        public GEPropertySetter(string refId, PropertyChangeType changeType, double value)
        {
            this.refId = refId;
            this.changeType = changeType;
            this.value = value;
        }

        public void Execute(GameElementManager elementManager)
        {
            GEProperty property = elementManager.GetProperty(refId);
            if (property == null)
            {
                Injector.Logger.LogWarn("There is no property element with the given id, however an action is reffering it! The given id: " + refId);
            }
            switch (changeType)
            {
                case PropertyChangeType.SET:
                    {
                        property.Value = this.value;
                        break;
                    }
                case PropertyChangeType.INC:
                    {
                        property.Value += this.value;
                        break;
                    }
                case PropertyChangeType.DEC:
                    {
                        property.Value -= this.value;
                        break;
                    }
            }
        }
    }
    [Serializable]
    public enum PropertyChangeType
    {
        SET, INC, DEC 
    }

    [Serializable]
    public class GESaveAction
    {
        private bool isAutoSave;
        private string saveStationId;

        public GESaveAction(bool isAutoSave, string saveStationId)
        {
            this.isAutoSave = isAutoSave;
            this.saveStationId = saveStationId;
        }

        public bool IsAutoSave
        {
            get
            {
                return isAutoSave;
            }
        }

        public void Execute()
        {
            if (isAutoSave)
            {
                GameSaverMenu.GetInstance().SaveGameWithTimetag(saveStationId);
            } else
            {
                GameController.GetInstance().OpenSaverMenu();
            }
        }
    }

    public int UseInterval
    {
        get
        {
            return useInterval;
        }
    }

    public GEText ResponseText
    {
        get
        {
            return responseText;
        }

        set
        {
            responseText = value;
        }
    }

    public List<GEActivation> Activations
    {
        get
        {
            return activations;
        }
    }

    public List<GEPropertySetter> PropertySetters
    {
        get
        {
            return propertySetters;
        }
    }

    public GESaveAction SaveAction
    {
        get
        {
            return saveAction;
        }
    }
}

