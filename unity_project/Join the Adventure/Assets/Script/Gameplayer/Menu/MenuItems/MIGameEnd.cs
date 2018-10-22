using System.Collections;
using System.Collections.Generic;

public class MIGameEnd : MenuItem
{
    public MIGameEnd(string menuText, MenuItemBundle parentBundle) : base(menuText, parentBundle)
    {
    }

    protected override void Execute()
    {
        GameController.GetInstance().HeadBackToGameMenu();
    }
}
