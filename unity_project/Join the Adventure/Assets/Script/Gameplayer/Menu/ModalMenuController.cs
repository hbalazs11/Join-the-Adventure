using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalMenuController : MonoBehaviour {

    public Text menuName;
    public GameObject buttonList;
    public GameObject buttonPrefab;

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
        this.gameObject.SetActive(value);
    }

    public void ChangeActivation()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        this.gameObject.SetActive(true);
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

    
}
