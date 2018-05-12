using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuItem {

    
    private GameObject thisMenuItem;
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
        thisMenuItem = MonoBehaviour.Instantiate(menuController.menuButtonPrefab) as GameObject;
        thisMenuItem.SetActive(true);

        thisMenuItem.transform.SetParent(menuController.buttonListGO.transform, false);

        thisMenuItem.GetComponent<Button>().onClick.AddListener(Execute);
        thisMenuItem.GetComponentInChildren<Text>().text = menuText;
        thisMenuItem.SetActive(false);
        return thisMenuItem;
    }

    public virtual void SetActive(bool value)
    {
        if(/*value &&*/ thisMenuItem == null)
        {
            InstantiateMenuButton();
        }
        isActive = value;
        if (thisMenuItem != null)
        {
            thisMenuItem.SetActive(value);
        }
    }

    public bool GetActive()
    {
        return isActive;
    }

    public void DestroyGO()
    {
        if (thisMenuItem == null) return;
        thisMenuItem.SetActive(true);
        MonoBehaviour.Destroy(thisMenuItem);
        thisMenuItem = null;
    }

    public string GetMenuText()
    {
        return menuText;
    }

    public void SetPositionNumber(int pos)
    {
        if (thisMenuItem == null) return;
        thisMenuItem.transform.SetSiblingIndex(pos);
    }

    public int GetPositionNumber()
    {
        if (thisMenuItem == null) return -1;
        return thisMenuItem.transform.GetSiblingIndex();
    }

    //public GameObject MenuItemGO
    //{
    //    get
    //    {
    //        return thisMenuItem;
    //    }
    //}

    protected abstract void Execute();
    
}
