using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenuItemBundle : MenuItemBundle
{

    MenuItem roomMenu;
    MenuItem inventoryMenu;
    MenuItem gotoMenu;
    //...

    public BaseMenuItemBundle() : base(LabelUtility.Instance.GetLabel(LabelNames.MENU), null)
    {
        inventoryMenu = MIOpenList<GEItem>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.INVENTORY), this, Injector.GameElementManager.Player.Items.Values);
    }

    public void SetRoom(GERoom room)
    {
        if (roomMenu != null)
        {
            roomMenu.DestroyGO();
        }
        if (gotoMenu != null)
        {
            gotoMenu.DestroyGO();
        }
        roomMenu = new MIOpenBundle<GERoom>(room.NameText.GetText(), room, this);
        gotoMenu = MIOpenList<GENeighbour>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.GONEXTROOM), this, room.Neighbours.Values);
    }

    

    public override bool IsActive
    {
        get
        {
            return this.isActive;
        }

        set
        {
            this.isActive = value;
            inventoryMenu.SetActive(value);
            roomMenu.SetActive(value);
            gotoMenu.SetActive(value);
            if (isActive)
            {
                RefreshInventoryMenu();
                menuController.SetMenuHeaderText(bundleName);
            }
        }
    }

    private void RefreshInventoryMenu()
    {
        if(inventoryMenu != null)
        {
            inventoryMenu.DestroyGO();
        }
        inventoryMenu = MIOpenList<GEItem>.CreateMIOpenList(LabelUtility.Instance.GetLabel(LabelNames.INVENTORY), this, Injector.GameElementManager.Player.Items.Values);
        inventoryMenu.SetActive(true);
        inventoryMenu.SetPositionNumber(0);
        if (!isActive)
        {
            inventoryMenu.SetActive(false);
        }
    }

}
