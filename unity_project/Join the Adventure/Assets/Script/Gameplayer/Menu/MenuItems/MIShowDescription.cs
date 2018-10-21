using System.Collections;
using System.Collections.Generic;

public class MIShowDescription : MenuItem
{
    private string descriptionText;
    private Description description;

    public MIShowDescription(string menuText, MenuItemBundle parentBundle, string descriptionText) : base(menuText, parentBundle)
    {
        this.descriptionText = descriptionText;
        description = Description.GetInstance();
    }

    protected override void Execute()
    {
        description.AddDescriptionText(descriptionText);
    }
}
