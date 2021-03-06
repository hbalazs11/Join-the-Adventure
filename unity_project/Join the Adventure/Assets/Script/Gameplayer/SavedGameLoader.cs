﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavedGameLoader : MonoBehaviour {
    
    public ModalMenuController loadMenuController;

    private bool startGameAfterSavedGameLoad;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () { 
        startGameAfterSavedGameLoad = false;
        InitLoadMenu();
    }
	
	// Update is called once per frame
	void Update () {
        if (startGameAfterSavedGameLoad)
        {
            startGameAfterSavedGameLoad = false;
            StartGame();
        }
    }

    public void StartGame()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        SceneManager.LoadScene("Game_Android");
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        SceneManager.LoadScene("Game_Standalone");
#endif
    }

    private void InitLoadMenu()
    {
        loadMenuController.SetMenuName(LabelUtility.Instance.GetLabel(LabelNames.SAVEDGAMES));
        foreach (string savedGameName in ObjectManager.CurrentGEM.savedGameNames)
        {
            loadMenuController.AddButton(savedGameName, delegate { LoadSavedGame(savedGameName); });
        }
    }

    private void LoadSavedGame(string savedGameName)
    {
        Thread loadingThread = new Thread(() => StartSavedGameLoading(ObjectManager.CurrentGEM.GameStorageName, savedGameName));
        loadingThread.Start();
    }

    private void StartSavedGameLoading(string gameName, string savedGameName)
    {
        List<string> savedGameNames = ObjectManager.CurrentGEM.savedGameNames;
        ObjectManager.CurrentGEM = PersistanceHelper.GetSavedGameGEM(gameName, savedGameName);
        ObjectManager.CurrentGEM.savedGameNames = savedGameNames;
        startGameAfterSavedGameLoad = true;
    }

    public void OpenLoadMenu()
    {
        RefreshNames();
        loadMenuController.OpenMenu();
    }

    private void RefreshNames()
    {
        List<string> currentNames = loadMenuController.GetContainedButtonNames();
        foreach (string saveName in ObjectManager.CurrentGEM.savedGameNames)
        {
            if (!currentNames.Contains(saveName))
            {
                loadMenuController.AddButton(saveName, delegate { LoadSavedGame(saveName); });
            }
        }
    }
}
