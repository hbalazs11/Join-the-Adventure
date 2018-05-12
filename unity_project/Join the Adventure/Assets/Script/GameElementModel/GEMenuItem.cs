using System.Collections;
using System.Collections.Generic;

public class GEMenuItem : ActivatableGameElement
{
    
    private int maxUseNumber;
    private GEText menuName;

    private List<GEAction> actions;
    private GERequirement requirements;
    private SortedList<string, GEText> texts;

    private int actionCounter;

    public GEMenuItem(string id, GEText menuName, List<GEAction> actions) : base(id)
    {
        this.menuName = menuName;
        this.actions = actions;
        isActive = true;
        maxUseNumber = 0; //infinit representations
        ActionCounter = 0;
    }

    public GEMenuItem(string id, GEText menuName, List<GEAction> actions, GERequirement requirements, SortedList<string, GEText> texts, bool? isActive, int? maxUseNumber) : this(id, menuName, actions)
    {
        this.requirements = requirements;
        this.texts = texts;
        this.maxUseNumber = maxUseNumber ?? 0;
        this.isActive = isActive ?? false;
    }


    public string Execute()
    {
        if(requirements != null && !requirements.Check())
        {
            return requirements.TextOnFail.GetText();
        }
        string response = GetCurrentActionset().Execute().GetText();
        ActionCounter++;
        return response;
    }

    private GEAction GetCurrentActionset()
    {
        GEAction selectedAction = actions[0];
        if(actions.Count == 1)
        {
            return selectedAction;
        }
        for(int i = 1; i < actions.Count; i++)
        {
            GEAction currentAction = actions[i];
            if (currentAction.UseInterval < ActionCounter) continue;
            if (selectedAction.UseInterval > currentAction.UseInterval)
            {
                selectedAction = currentAction;
            }
        }
        //if (selectedAction.UseInterval < ActionCounter) return null;
        return selectedAction;
    }

    public GEText MenuName
    {
        get
        {
            return menuName;
        }
        set
        {
            menuName = value;
        }
    }

    public int ActionCounter
    {
        get
        {
            return actionCounter;
        }

        set
        {
            actionCounter = value;
            if(maxUseNumber != 0 && actionCounter >= maxUseNumber)
            {
                SetActive(false);
            }
        }
    }
}
