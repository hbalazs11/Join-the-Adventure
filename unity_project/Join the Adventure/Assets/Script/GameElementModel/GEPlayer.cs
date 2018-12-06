using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GEPlayer  {

    private SortedList<string, GEProperty> properties;
    private SortedList<string, GEItem> items;
    private bool isFinal;

    public GEPlayer(SortedList<string, GEProperty> properties, SortedList<string, GEItem> items, bool isFinal = false)
    {
        this.properties = properties;
        Items = items;
        this.isFinal = isFinal; 
    }

    public bool IsFinal
    {
        get { return isFinal; }
    }

    public SortedList<string, GEItem> Items
    {
        get
        {
            return items;
        }

        set
        {
            if (items != null)
            {
                foreach (GEItem item in items.Values)
                {
                    item.IsEquipped = false;
                }
            }
            items = value;
            foreach (GEItem item in items.Values)
            {
                item.IsEquipped = true;
            }
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
}
