using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIEquip : MenuItem
{
    private GameElementManager elementManager;
    private GEItem geItem;
    private Description description;

    public MIEquip(GEItem geItem, MenuItemBundle parentBundle) : base(LabelUtility.Instance.GetLabel(LabelNames.PICKUP), parentBundle)
    {
        elementManager = ObjectManager.CurrentGEM;
        this.geItem = geItem;
        description = Description.GetInstance();
    }

    protected override void Execute()
    {
        geItem.Equip(elementManager);
        parentBundle.RemoveItem(this);
        string itemName = geItem.ItemName.GetText();
        parentBundle.Parent.RemoveItemByName(itemName);
        description.AddDescriptionText(LabelUtility.Instance.GetLabel(LabelNames.PICKUPNOTIF) + " " + itemName);
    }

    public override void SetActive(bool value)
    {
        if (value)
        {
            base.SetActive(geItem.IsActive() && geItem.Equipable);
        }
        else
        {
            base.SetActive(value);
        }
    }
}
