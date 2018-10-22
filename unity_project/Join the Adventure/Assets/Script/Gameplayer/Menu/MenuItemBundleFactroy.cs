using System.Collections.Generic;

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

        //if (room.Npcs.Values.Count != 0)
        //{
        //}
        newBundle.AddMenuItem(MIOpenList<GENpc>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.NPCS), newBundle, room.Npcs.Values));

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
        foreach (GENeighbour neighbour in geNeighbours)
        {
            newBundle.AddMenuItem(new MIRoomChange(neighbour.MenuText.GetText(), newBundle, neighbour));
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateBundle(GENpc npc, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(npc.NameText.GetText(), parent);
        
        newBundle.AddMenuItem(new MIShowDescription(LabelUtility.Instance.GetLabel(LabelNames.SHOWDESCRUPTION), newBundle, npc.DescText.GetText()));
        newBundle.AddMenuItem(new MIStartConversation(LabelUtility.Instance.GetLabel(LabelNames.STARTCONVERSATION), newBundle, npc));

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }

    public static MenuItemBundle CreateListBundle(string bundleName, List<GENpc> geNpcs, MenuItemBundle parent)
    {
        MenuItemBundle newBundle = new MenuItemBundle(bundleName, parent);
        
        foreach (GENpc npc in geNpcs)
        {
            newBundle.AddMenuItem(new MIOpenBundle<GENpc>(npc.NameText.GetText(), npc, newBundle));
            npc.OnActivationChange += newBundle.RefreshOnEvent;
        }

        newBundle.AddMenuItem(new MIBack(parent));

        return newBundle;
    }
}
