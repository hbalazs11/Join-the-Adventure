using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public abstract class GameElement  {

    protected string id;

    public GameElement(string id)
    {
        this.id = id;
    }

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }
    
}
