using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuItem {

    
    private GameObject thisGOMenuItem;
    protected MenuController menuController;
    protected MenuItemBundle parentBundle;

    protected string menuText;

    private bool isActive;

    protected MenuItem(string menuText, MenuItemBundle parentBundle)
    {
        this.menuText = menuText;
        this.parentBundle = parentBundle;
        menuController = MenuController.GetInstance();
    }

    public GameObject InstantiateMenuButton()
    {
        thisGOMenuItem = MonoBehaviour.Instantiate(menuController.menuButtonPrefab) as GameObject;

        thisGOMenuItem.SetActive(true);
        thisGOMenuItem.transform.SetParent(menuController.buttonListGO.transform, false);

        thisGOMenuItem.GetComponent<Button>().onClick.AddListener(Execute);
        thisGOMenuItem.GetComponentInChildren<Text>().text = menuText;
        thisGOMenuItem.SetActive(false);
        return thisGOMenuItem;
    }

    public virtual void SetActive(bool value)
    {
        Debug.Log("MenuItem.SetActive() - value: " + value + ", itemName: " + menuText);
        if(/*value &&*/ thisGOMenuItem == null)
        {
            InstantiateMenuButton();
            Debug.Log("MenuItem.SetActive() - instantiation... [" + menuText + "]");
        }
        isActive = value;
        if (thisGOMenuItem != null)
        {
            thisGOMenuItem.SetActive(value);
            Debug.Log("MenuItem.SetActive() - GOActivation... [" + menuText + "]");
        }
    }

    public bool GetActive()
    {
        return isActive;
    }

    public void DestroyGO()
    {
        if (thisGOMenuItem == null) return;
        thisGOMenuItem.SetActive(true);
        MonoBehaviour.Destroy(thisGOMenuItem);
        thisGOMenuItem = null;
    }

    public string GetMenuText()
    {
        return menuText;
    }

    public void SetPositionNumber(int pos)
    {
        if (thisGOMenuItem == null) return;
        thisGOMenuItem.transform.SetSiblingIndex(pos);
    }

    public int GetPositionNumber()
    {
        if (thisGOMenuItem == null) return -1;
        return thisGOMenuItem.transform.GetSiblingIndex();
    }

    public GameObject MenuItemGO
    {
        get
        {
            return thisGOMenuItem;
        }
    }

    protected abstract void Execute();
    
}
