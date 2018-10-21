using System.Collections;
using System.Collections.Generic;

public class GEGameEnd : GameElement {
    private GEText endText;

    public GEGameEnd(string id) : base(id)
    {
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
