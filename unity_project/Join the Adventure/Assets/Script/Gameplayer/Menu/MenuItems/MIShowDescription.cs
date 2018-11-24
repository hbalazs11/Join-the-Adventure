using System;
using System.Collections;
using System.Collections.Generic;

public class MIShowDescription : MenuItem
{
    private string descriptionText;
    private Description description;
    private Func<string> getDescText;

    public MIShowDescription(string menuText, MenuItemBundle parentBundle, string descriptionText) : base(menuText, parentBundle)
    {
        this.descriptionText = descriptionText;
        description = Description.GetInstance();
    }

    public MIShowDescription(string menuText, MenuItemBundle parentBundle, Func<string> getDescText) : base(menuText, parentBundle)
    {
        description = Description.GetInstance();
        this.getDescText = getDescText;
    } 

    protected override void Execute()
    {
        if (descriptionText != null)
        {
            description.AddDescriptionText(descriptionText);
        } else if( getDescText != null)
        {
            description.AddDescriptionText(getDescText());
        }
    }
}
