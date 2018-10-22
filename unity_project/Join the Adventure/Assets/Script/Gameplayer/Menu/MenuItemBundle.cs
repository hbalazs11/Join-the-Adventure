using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemBundle  {

    protected string bundleName;
    protected bool isActive;
    protected MenuController menuController;
    private List<MenuItem> menuItems;
    private MenuItemBundle parent;
    public event EventHandler<EventArgs> OnExecutionSidefects;


    public MenuItemBundle(string bundleName, MenuItemBundle parent)
    {
        this.bundleName = bundleName;
        menuItems = new List<MenuItem>();
        menuController = MenuController.GetInstance();
        this.parent = parent;
    }

    public void ExecuteSideEffects()
    {
        if (OnExecutionSidefects != null)
        {
            OnExecutionSidefects(null, EventArgs.Empty);
        }
    }

    public virtual bool IsActive {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            SetItemActivations(isActive);
            if (isActive)
            {
                menuController.SetMenuHeaderText(bundleName);
            }
        }
    }

    private void SetItemActivations(bool value)
    {
        foreach(MenuItem item in menuItems)
        {
            item.SetActive(value);
        }
    }

    public void RefreshBundle()
    {
        SetItemActivations(isActive);
    }

    public void RemoveItem(MenuItem itemToRemove)
    {
        if (!menuItems.Contains(itemToRemove))
        {
            return;
        }
        itemToRemove.DestroyGO();
        menuItems.Remove(itemToRemove);
    }

    public void RemoveItemByName(string itemName)
    {
        MenuItem itemToRemove = null;
        foreach(MenuItem item in menuItems)
        {
            if (item.GetMenuText().Equals(itemName))
            {
                itemToRemove = item;
                break;
            }
        }
        if(itemToRemove != null)
        {
            RemoveItem(itemToRemove);
        }
    }

    public void RefreshOnEvent(object o, EventArgs e)
    {
        RefreshBundle();
    }

    public void DestroyItems()
    {
        foreach (MenuItem item in menuItems)
        {
            item.DestroyGO();
        }
    }


    public string Name
    {
        get
        {
            return this.bundleName;
        }
    }

    public MenuItemBundle Parent
    {
        get
        {
            return parent;
        }
    }

    public void AddMenuItem(MenuItem item)
    {
        menuItems.Add(item);
    }
}
