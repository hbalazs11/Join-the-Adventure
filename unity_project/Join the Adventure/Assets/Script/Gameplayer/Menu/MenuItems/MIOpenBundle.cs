using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIOpenBundle<T> : MenuItem where T : GameElement
{
    MenuItemBundle bundleInstance;
    T element;

    public MIOpenBundle(string menuText, T element, MenuItemBundle parentBundle) : base(menuText, parentBundle)
    {
        this.element = element;
    }

    protected override void Execute()
    {
        if(bundleInstance == null)
        {
            if (element is GERoom)
            {
                bundleInstance = MenuItemBundleFactroy.CreateBundle(element as GERoom, parentBundle);
            } else if(element is GEItem)
            {
                bundleInstance = MenuItemBundleFactroy.CreateBundle(element as GEItem, parentBundle);
            } else if (element is GENpc)
            {
                bundleInstance = MenuItemBundleFactroy.CreateBundle(element as GENpc, parentBundle);
            } else
            {
                //...
            }
        }
        menuController.CurrentBundle = bundleInstance;
    }

    public override void SetActive(bool value)
    {
        if (element is IActivatable)
        {
            if (value)
            {
                base.SetActive((element as IActivatable).IsActive() == true);
            }
            else
            {
                base.SetActive(value);
            }
        }
        else
        {
            base.SetActive(value);
        }
    }
}
