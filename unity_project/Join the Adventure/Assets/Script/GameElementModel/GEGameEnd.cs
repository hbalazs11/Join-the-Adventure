using System.Collections;
using System.Collections.Generic;

public class GEGameEnd : ActivatableGameElement {
    private GEText endText;

    public GEGameEnd(string id) : base(id)
    {
        OnActivationChange += GEGameEnd_OnActivationChange;
    }

    private void GEGameEnd_OnActivationChange(object sender, System.EventArgs e)
    {
        if (!isActive) return;
        Description description = Description.GetInstance();
        description.ClearDescription();
        description.AddDescriptionText(endText.GetText());
        
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
}
