using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemBundleFactroy  {
    
	public static MenuItemBundle CreateBundle(GERoom room, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(room.NameText.GetText(), parent);
        foreach(GEMenuItem action in room.MenuItems.Values)
        {
            newBundle.AddMenuItem(new MIGameElementAction(action.MenuName.GetText(), newBundle, action));
            action.OnActivationChange += newBundle.RefreshOnEvent;
        }
        newBundle.AddMenuItem(MIOpenList<GEItem>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.ITEMS), newBundle, room.Items.Values));

        //TODO: npcs...

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateBundle(GEItem item, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(item.ItemName.GetText(), parent);
        if (item.Equipable && !item.IsEquipped)
        {
            newBundle.AddMenuItem(new MIEquip(item, newBundle));
        }
        foreach (GEMenuItem action in item.MenuItems.Values)
        {
            newBundle.AddMenuItem(new MIGameElementAction(action.MenuName.GetText(), newBundle, action));
            action.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }
    

    public static MenuItemBundle CreateListBundle(string bundleName, List<GEItem> geItems, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        if(geItems == null || geItems.Count == 0)
        {
            return newBundle;
        }
        foreach (GEItem item in geItems)
        {
            newBundle.AddMenuItem(new MIOpenBundle<GEItem>(item.ItemName.GetText(), item, newBundle));
            item.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateListBundle(string bundleName, List<GENeighbour> geNeighbours, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        if (geNeighbours == null || geNeighbours.Count == 0)
        {
            return newBundle;
        }
        foreach (GENeighbour neighbour in geNeighbours)
        {
            newBundle.AddMenuItem(new MIRoomChange(neighbour.MenuText.GetText(), newBundle, neighbour));
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

}
