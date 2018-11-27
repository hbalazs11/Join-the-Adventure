using System;

[Serializable]
public class GENeighbour : ActivatableGameElement
{

    private GERequirement requirements;

    public GENeighbour(string id, bool active) : base(id)
    {
        isActive = active;
    }

    public GERoom Room { get; set; }
    public GEText MenuText { get; set; }

    public GERequirement Requirements
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
}