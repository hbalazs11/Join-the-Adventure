using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GEGameEnd : ActivatableGameElement {
    private GEText endText;
    private GEText gameOverText;

    public GEGameEnd(string id) : base(id)
    {
    }

    private void OnActivation()
    {
        if (!isActive) return;
        Description description = Description.GetInstance();
        description.ClearDescription();
        description.AddDescriptionText(endText.GetText());
        GameController.GetInstance().ShowGameOver(gameOverText.GetText());
        MenuItemBundle menuItemBundle = new MenuItemBundle("", null);
        menuItemBundle.AddMenuItem(new MIGameEnd("OK",null));
        MenuController.GetInstance().CurrentBundle = menuItemBundle;
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        OnActivation();
    }

    public GEText EndText
    {
        get
        {
            return endText;
        }

        set
        {
            endText = value;
        }
    }

    public GEText GameOverText
    {
        get
        {
            return gameOverText;
        }

        set
        {
            gameOverText = value;
        }
    }
}
