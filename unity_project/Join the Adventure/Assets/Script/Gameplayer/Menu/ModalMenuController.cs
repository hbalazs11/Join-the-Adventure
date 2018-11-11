using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalMenuController : MonoBehaviour {

    public Text menuName;
    public GameObject buttonList;
    public GameObject buttonPrefab;
    public ModalMenuController parentModalController;

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
        if(parentModalController != null)
        {
            parentModalController.OpenMenu();
        }
    }

    public void OpenMenu()
    {
        this.gameObject.SetActive(true);
        if (parentModalController != null)
        {
            parentModalController.CloseMenu();
        }
    }

    public void AddButton(string label, UnityEngine.Events.UnityAction call)
    {
        var langButton = MonoBehaviour.Instantiate(buttonPrefab) as GameObject;

        langButton.SetActive(true);
        langButton.transform.SetParent(buttonList.transform, false);

        langButton.GetComponent<Button>().onClick.AddListener(call);
        langButton.GetComponentInChildren<Text>().text = label;
        langButton.SetActive(true);
    }

    public void AddBackButton(string label = "Back")
    {
        AddButton(label, CloseMenu);
    }

    public void SetParentModalController(ModalMenuController parent)
    {
        this.parentModalController = parent;
    }
}
