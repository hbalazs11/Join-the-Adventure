using System;
using System.Collections.Generic;

[Serializable]
class GEGameProperties
{
    public string firstRoomId;
    public string defLang;
    private GEText gameNameText;
    private GEText greetingText;
    private bool checkpointsOn;
    private string menuImgSrc;

    public bool IsMenuSaveAvailable { get; set; }

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText, string menuImgSrc)
    {
        this.firstRoomId = firstRoomId;
        this.defLang = defLang;
        this.gameNameText = gameNameText;
        IsMenuSaveAvailable = true;
        this.checkpointsOn = false;
        this.menuImgSrc = menuImgSrc;
    }

    public GEGameProperties(string firstRoomId, string defLang, GEText gameNameText, string menuImgPath, GEText greetingText, bool isMenuSaveAvailable, bool checkpointsOn) : this(firstRoomId, defLang, gameNameText, menuImgPath)
    {
        this.greetingText = greetingText;
        this.checkpointsOn = checkpointsOn;
        IsMenuSaveAvailable = isMenuSaveAvailable;
    }

    public string MenuImgSrc
    {
        get
        {
            return menuImgSrc;
        }
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

    public bool IsCheckpointOn
    {
        get
        {
            return checkpointsOn;
        }
    }
}

