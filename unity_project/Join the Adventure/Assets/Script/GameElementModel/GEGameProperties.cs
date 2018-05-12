using System;
using System.Collections.Generic;


class GEGameProperties
{
    public string firstRoomId;
    public string defLang;
    private GEText gameNameText;
    private GEText greetingText;
    public bool checkpointsOn;

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText)
    {
        this.firstRoomId = firstRoomId;
        this.defLang = defLang;
        this.gameNameText = gameNameText;
    }

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText, GEText greetingText, bool checkpointsOn) : this(firstRoomId, defLang, gameNameText)
    {
        this.greetingText = greetingText;
        this.checkpointsOn = checkpointsOn;
    }
    
    public GEText GreetingText
    {
        get
        {
            return greetingText;
        }

        set
        {
            greetingText = value;
        }
    }

    public GEText GameNameText
    {
        get
        {
            return gameNameText;
        }

        set
        {
            gameNameText = value;
        }
    }
}

