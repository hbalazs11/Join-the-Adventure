using System.Collections;
using System.Collections.Generic;

public class GEPlayer  {

    private SortedList<string, GEProperty> properties;
    private SortedList<string, GEItem> items;

    public GEPlayer(SortedList<string, GEProperty> properties, SortedList<string, GEItem> items)
    {
        this.properties = properties;
        Items = items;
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
