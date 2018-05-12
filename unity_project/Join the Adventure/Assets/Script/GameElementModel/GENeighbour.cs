public class GENeighbour : ActivatableGameElement
{

    private GERequirement requirements;

    public GENeighbour(string id) : base(id)
    {
        isActive = true;
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