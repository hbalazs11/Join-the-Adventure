using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIGameElementAction : MenuItem
{
    private GEMenuItem geMenuItem;
    private Description description;

    public MIGameElementAction(string menuText, MenuItemBundle parentBundle, GEMenuItem geMenuItem) : base(menuText, parentBundle)
    {
        this.geMenuItem = geMenuItem;
        description = Description.GetInstance();
    }

    protected override void Execute()
    {
        string response = geMenuItem.Execute();
        if(response.Length != 0)
            description.AddDescriptionText(response);
    }

    public override void SetActive(bool value)
    {
        if(value)
        {
            base.SetActive(geMenuItem.IsActive() == true);
        } else
        {
            base.SetActive(value);
        }
    }
}
