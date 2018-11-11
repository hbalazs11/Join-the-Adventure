﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ModalMenuController))]
public class GameSaverMenu : MonoBehaviour {

    public InputField inputField;
    public Text inputPlaceholder;
    public Button saveButton;

    private ModalMenuController menuController;
    private GameElementManager elementManager;

    private readonly object syncLock = new object();

    private bool saveInAction = false;
    private bool saveFinished = false;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenMenu(GameElementManager gem)
    {
        if (elementManager == null || elementManager != gem)
        {
            InitMenu(gem);
        } else {
            RefreshNames();
        }
        inputField.text = "";
        menuController.OpenMenu();
    }

    private void RefreshNames()
    {
        List<string> currentNames = menuController.GetContainedButtonNames();
        foreach (string saveName in elementManager.savedGameNames)
        {
            if (!currentNames.Contains(saveName))
            {
                menuController.AddButton(saveName, delegate { SetSaveName(saveName); });
            }
        }
    }

    private void InitMenu(GameElementManager gem)
    {
        elementManager = gem;
        menuController = this.gameObject.GetComponent<ModalMenuController>();
        saveButton.onClick.AddListener(SaveGame);
        saveButton.GetComponentInChildren<Text>().text = LabelUtility.Instance.GetLabel(LabelNames.SAVE);
        menuController.SetMenuName(LabelUtility.Instance.GetLabel(LabelNames.SAVE));
        inputPlaceholder.text = LabelUtility.Instance.GetLabel(LabelNames.SAVENAME);
        foreach (string savedGameName in Injector.GameElementManager.savedGameNames)
        {
            menuController.AddButton(savedGameName, delegate { SetSaveName(savedGameName); });
        }
    }

    private void SetSaveName(string name)
    {
        inputField.text = name;
    }

    private void SaveGame()
    {
        if (inputField.text.Length == 0) return;
        Thread loadingThread = new Thread(() => StartSaveOnAnotherThread(Injector.GameElementManager.GameStorageName, inputField.text, Injector.GameElementManager));
        saveInAction = true;
        loadingThread.Start();
        menuController.CloseMenu();
    }

    private void StartSaveOnAnotherThread(string gameName, string saveName, GameElementManager gem)
    {
        PersistanceHelper.StoreSavedGameGEM(gameName, saveName, gem);
        saveInAction = false;
        saveFinished = true;
        lock (syncLock)
        {
            if (!gem.savedGameNames.Contains(saveName))
            {
                gem.savedGameNames.Add(saveName);
            }
        }
    }
}