using System.Collections;
using System.Collections.Generic;

public class GEMenuItem : GameElement, IActivatable
{
    private bool isActive;
    
    private int? useNumber;
    private GEText menuName;

    private Dictionary<string, GEAction> actions;
    private Dictionary<string, GERequirement> requirements;
    private Dictionary<string, GEText> texts;

    public GEMenuItem(string id, GEText menuName, Dictionary<string, GEAction> actions) : base(id)
    {
        this.menuName = menuName;
        this.actions = actions;
    }


    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool active)
    {
        this.isActive = active;
    }
}
