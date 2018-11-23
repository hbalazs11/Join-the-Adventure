using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredGameRemover : MonoBehaviour {

    public Button saveButton;

    private bool initiated = false;
    private List<string> storedGameNames;
    private List<string> selectedGameNames;

    private ModalMenuController menuController;

    private ColorBlock selectedColorBlock;
    private ColorBlock defaultColorBlock;
    private Color selectedColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenMenu()
    {
        if (!initiated)
        {
            InitMenu();
        }
        menuController.OpenMenu(false);
    }
    

    public void InitMenu()
    {
        menuController = this.gameObject.GetComponent<ModalMenuController>();
        saveButton.onClick.AddListener(DeleteSelectedGames);
        storedGameNames = PersistanceHelper.GetStoredGameNames();
        selectedGameNames = new List<string>();
        foreach (string storedGameName in storedGameNames)
        {
            menuController.AddButton(storedGameName, delegate { HandleClickOnGameName(storedGameName); });
        }
        if(storedGameNames.Count != 0)
        {
            InitColors(storedGameNames[0]);
        }
        initiated = true;
    }

    private void HandleClickOnGameName(string gameName)
    {
        if (selectedGameNames.Contains(gameName))
        {
            selectedGameNames.Remove(gameName);

            menuController.GetButton(gameName).colors = defaultColorBlock;
        } else
        {
            selectedGameNames.Add(gameName);
            menuController.GetButton(gameName).colors = selectedColorBlock;
        }
    }

    private void InitColors(string buttonLabel)
    {
        //selectedColor = new Color(66, 217, 244);
        selectedColor = Color.red;
        defaultColorBlock = menuController.GetButton(buttonLabel).colors;
        selectedColorBlock = defaultColorBlock;
        selectedColorBlock.normalColor = selectedColor;
    }

    private void DeleteSelectedGames()
    {
        foreach(string selectedName in selectedGameNames)
        {
            PersistanceHelper.RemoveStoredGame(selectedName);
            storedGameNames.Remove(selectedName);
            menuController.RemoveButton(selectedName);
        }
    }
}
