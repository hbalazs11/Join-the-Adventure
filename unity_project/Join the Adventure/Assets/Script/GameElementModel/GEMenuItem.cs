using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
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
        actions.Sort((a1, a2) =>
        {
            return a1.UseInterval - a2.UseInterval;
        });
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
        if (actions.Count == 1)
        {
            return actions[0];
        }
        foreach(GEAction act in actions)
        {
            if (act.UseInterval < ActionCounter + 1) continue;
            return act;
        }
        return actions[actions.Count-1];
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
}
