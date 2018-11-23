using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject menuButtonPrefab;
    public GameObject buttonListGO;

    public Text menuHeader;

    BaseMenuItemBundle baseBundle;
    MenuItemBundle currentBundle;

    List<MenuItemBundle> usedBundles;

    private CanvasGroup canvasGroup;

    public static MenuController GetInstance()
    {
        return FindObjectOfType<MenuController>();
    }

    protected MIBack ActiveBackMI { get; set; }

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
            ActiveBackMI = currentBundle.BackButton;
            SetDefaultMenuButtonSelection();
        }
    }

    void Awake()
    {
        baseBundle = new BaseMenuItemBundle();
        usedBundles = new List<MenuItemBundle>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!canvasGroup.interactable)
            return;
#if UNITY_ANDROID //&& !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ActiveBackMI != null)
            {
                ActiveBackMI.ExecuteBack();
            }
        }
#endif
#if UNITY_STANDALONE //|| UNITY_EDITOR
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (ActiveBackMI != null)
            {
                ActiveBackMI.ExecuteBack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.GetInstance().GameMMOpenMenu();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(canvasGroup.interactable && EventSystem.current.currentSelectedGameObject == null)
            {
                SetDefaultMenuButtonSelection();
            }
        }
#endif
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

    public void SetDefaultMenuButtonSelection()
    {
#if UNITY_STANDALONE //|| UNITY_EDITOR
        if (currentBundle.GetFirstActiveMenuItem() != null)
        {
            EventSystem.current.SetSelectedGameObject(currentBundle.GetFirstActiveMenuItem().MenuItemGO);
        }
#endif
    }
}
