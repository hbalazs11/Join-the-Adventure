using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIRoomChange : MenuItem
{

    private GENeighbour geNeighbour;
    private Description description;
    private GameController gameController;

    public MIRoomChange(string menuText, MenuItemBundle parentBundle, GENeighbour geNeighbour) : base(menuText, parentBundle)
    {
        this.geNeighbour = geNeighbour;
        description = Description.GetInstance();
        gameController = GameController.GetInstance();
    }

    protected override void Execute()
    {
        if (geNeighbour.Requirements == null)
        {
            gameController.LoadRoom(geNeighbour.Room);
        }
        else
        {
            if (geNeighbour.Requirements.Check())
            {
                gameController.LoadRoom(geNeighbour.Room);
            }
            else
            {
                description.AddDescriptionText(geNeighbour.Requirements.TextOnFail.GetText());
            }
        }
    }


    public override void SetActive(bool value)
    {
        if (value)
        {
            base.SetActive(geNeighbour.IsActive() == true);
        }
        else
        {
            base.SetActive(value);
        }

    }
}
