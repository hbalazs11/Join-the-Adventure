using System.Collections;
using System.Collections.Generic;

public class GEItem : GameElement, IActivatable{

    private Dictionary<string, GEMenuItem> menuItems;
    private Dictionary<string, GEProperty> properties;
    private Dictionary<string, GEText> texts;
    private bool isActive;
    private bool equipable;
    private GEText description;
    private GEText itemName;

    public GEItem(string id, GEText itemName): base(id)
    {
        menuItems = new Dictionary<string, GEMenuItem>();
        properties = new Dictionary<string, GEProperty>();
        texts = new Dictionary<string, GEText>();
        this.itemName = itemName;
    }

    public GEItem(string id, GEText itemName, bool isActive, bool equipable, GEText description) : this(id, itemName)
    {
        this.isActive = isActive;
        this.equipable = equipable;
        this.description = description;
    }

    public Dictionary<string, GEMenuItem> MenuItems
    {
        get
        {
            return menuItems;
        }

        set
        {
            menuItems = value;
        }
    }

    public Dictionary<string, GEProperty> Properties
    {
        get
        {
            return properties;
        }

        set
        {
            properties = value;
        }
    }

    public Dictionary<string, GEText> Texts
    {
        get
        {
            return texts;
        }

        set
        {
            texts = value;
        }
    }

    public bool Equipable
    {
        get
        {
            return equipable;
        }
    }

    public GEText Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public GEText ItemName
    {
        get
        {
            return itemName;
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool active)
    {
        this.isActive = active;
    }
}
