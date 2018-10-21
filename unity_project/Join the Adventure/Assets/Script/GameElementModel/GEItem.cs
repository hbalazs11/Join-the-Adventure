using System;
using System.Collections;
using System.Collections.Generic;

public class GEItem : ActivatableGameElement
{

    private SortedList<string, GEMenuItem> menuItems;
    private SortedList<string, GEProperty> properties;
    private SortedList<string, GEText> texts;
    private bool isEquipped;
    private bool equipable;
    private GEText description;
    private GEText itemName;
    

    public GEItem(string id, GEText itemName): base(id)
    {
        menuItems = new SortedList<string, GEMenuItem>();
        properties = new SortedList<string, GEProperty>();
        texts = new SortedList<string, GEText>();
        this.itemName = itemName;
        isEquipped = false;
    }

    public GEItem(string id, GEText itemName, bool isActive, bool equipable, GEText description) : this(id, itemName)
    {
        this.isActive = isActive;
        this.equipable = equipable;
        this.description = description;
    }

    public void Equip(GameElementManager elementManager)
    {
        if(equipable && !isEquipped)
        {
            elementManager.Player.Items.Add(id, this);
            isEquipped = true;
        }
    }

    public void Unequip(GameElementManager elementManager)
    {
        if (equipable && isEquipped)
        {
            elementManager.Player.Items.Remove(id);
            isEquipped = false;
        }
    }

    public SortedList<string, GEMenuItem> MenuItems
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

    public SortedList<string, GEProperty> Properties
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

    public SortedList<string, GEText> Texts
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
        set
        {
            this.itemName = value;
        }
    }

    public bool IsEquipped
    {
        get
        {
            return isEquipped;
        }

        set
        {
            isEquipped = value;
        }
    }

    
}
