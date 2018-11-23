using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalMenuController : MonoBehaviour {

    public Text menuName;
    public GameObject buttonList;
    public GameObject buttonPrefab;
    public ModalMenuController parentModalController;
    public List<CanvasGroup> blockedCanvasGroups;

    private bool openedFromParent = false;
    private Dictionary<string, GameObject> menuButtons = new Dictionary<string, GameObject>();

    //void Awake()
    //{
    //    buttonList = this.gameObject.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
    //}

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void SetMenuName(string name)
    {
        menuName.text = name;
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            OpenMenu();
        } else
        {
            CloseMenu();
        }
    }

    public void ChangeActivation()
    {
        if (this.gameObject.activeSelf)
        {
            CloseMenu();
        } else
        {
            OpenMenu();
        }
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
        if(parentModalController != null && openedFromParent)
        {
            parentModalController.OpenMenu();
        }
        SetCanvasesInteractable(blockedCanvasGroups, true);
    }

    public void OpenMenu(bool fromParent = true)
    {
        this.gameObject.SetActive(true);
        openedFromParent = fromParent;
        if (parentModalController != null && fromParent)
        {
            parentModalController.CloseMenu();
            SetCanvasesInteractable(parentModalController.blockedCanvasGroups, false);
        }
        SetCanvasesInteractable(blockedCanvasGroups, false);
    }

    private void SetCanvasesInteractable(List<CanvasGroup> canvasGroups, bool interactable)
    {
        if (canvasGroups != null)
        {
            foreach (CanvasGroup cg in canvasGroups)
            {
                cg.interactable = interactable;
            }
        }
    }

    public void AddButton(string label, UnityEngine.Events.UnityAction call)
    {
        var newButton = MonoBehaviour.Instantiate(buttonPrefab) as GameObject;

        newButton.SetActive(true);
        newButton.transform.SetParent(buttonList.transform, false);

        newButton.GetComponent<Button>().onClick.AddListener(call);
        newButton.GetComponentInChildren<Text>().text = label;
        newButton.SetActive(true);
        menuButtons.Add(label, newButton);
    }

    public void AddBackButton(string label = "Back")
    {
        AddButton(label, CloseMenu);
    }

    public void SetParentModalController(ModalMenuController parent)
    {
        this.parentModalController = parent;
    }

    public List<string> GetContainedButtonNames()
    {
        return new List<string>(menuButtons.Keys);
    }
}
