using System;
using System.Collections.Generic;

[Serializable]
class GEGameProperties
{
    public string firstRoomId;
    public string defLang;
    private GEText gameNameText;
    private GEText greetingText;
    public bool checkpointsOn;

    public bool IsMenuSaveAvailable { get; set; }

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText)
    {
        this.firstRoomId = firstRoomId;
        this.defLang = defLang;
        this.gameNameText = gameNameText;
        IsMenuSaveAvailable = true;
        this.checkpointsOn = false;
    }

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText, GEText greetingText, bool isMenuSaveAvailable, bool checkpointsOn) : this(firstRoomId, defLang, gameNameText)
    {
        this.greetingText = greetingText;
        this.checkpointsOn = checkpointsOn;
        IsMenuSaveAvailable = isMenuSaveAvailable;
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

