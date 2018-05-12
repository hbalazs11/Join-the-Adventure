using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject menuButtonPrefab;
    public GameObject buttonListGO;

    public Text menuHeader;

    BaseMenuItemBundle baseBundle;
    MenuItemBundle currentBundle;

    List<MenuItemBundle> usedBundles;

    public static MenuController GetInstance()
    {
        return FindObjectOfType<MenuController>();
    }

    public MenuItemBundle CurrentBundle
    {
        get
        {
            return currentBundle;
        }
        set
        {
            if (currentBundle != null)
            {
                currentBundle.IsActive = false;
            }
            currentBundle = value;
            currentBundle.IsActive = true;
            if (!currentBundle.Equals(baseBundle) && !usedBundles.Contains(currentBundle))
            {
                usedBundles.Add(currentBundle);
            }
        }
    }

    void Awake()
    {
        baseBundle = new BaseMenuItemBundle();
        usedBundles = new List<MenuItemBundle>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		   //(this.transform as RectTransform).
	}

    public void LoadRoom(GERoom room)
    {
        baseBundle.SetRoom(room);
        CurrentBundle = baseBundle;
        foreach(MenuItemBundle bundle in usedBundles)
        {
            bundle.DestroyItems();
        }
        usedBundles.Clear();
    }

    public void SetMenuHeaderText(string text)
    {
        menuHeader.text = text;
    }
}
