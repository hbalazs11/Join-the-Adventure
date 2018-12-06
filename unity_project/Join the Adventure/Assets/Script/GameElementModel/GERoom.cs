using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GERoom : ActivatableGameElement
{ 
    private GEText nameText;
    private GEText descText;
    private string imgPath;
    private bool isCheckPoint;
    private bool isVisited;

    public GERoom(string id, GEText nameText, string imgPath) : base(id)
    {
        this.nameText = nameText;
        this.imgPath = imgPath;
        this.isActive = true;
        this.isCheckPoint = false;
        this.isVisited = false;
    }

    public SortedList<string, GEProperty> Properties { get; set; }
    public SortedList<string, GEMenuItem> MenuItems { get; set; }
    public SortedList<string, GEItem> Items { get; set; }
    public SortedList<string, GENpc> Npcs { get; set; }
    public SortedList<string, GEText> Texts { get; set; }
    public SortedList<string, GENeighbour> Neighbours { get; set; }

    public GERoom(string id, GEText nameText, string imgPath, GEText descText, bool isActive, bool isCheckpoint) : this(id,nameText,imgPath)
    {
        this.descText = descText;
        this.isActive = isActive;
        this.isCheckPoint = isCheckpoint;
    }

    public GEText NameText
    {
        get
        {
            return nameText;
        }

        set
        {
            nameText = value;
        }
    }

    public string ImgPath
    {
        get
        {
            return imgPath;
        }

        set
        {
            imgPath = value;
        }
    }

    public bool IsCheckPoint
    {
        get
        {
            return isCheckPoint;
        }

        set
        {
            isCheckPoint = value;
        }
    }

    public GEText DescText
    {
        get
        {
            return descText;
        }

        set
        {
            descText = value;
        }
    }

    public bool IsVisited
    {
        get
        {
            return isVisited;
        }

        set
        {
            isVisited = value;
        }
    }
}
