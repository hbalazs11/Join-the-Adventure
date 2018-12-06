﻿using GracesGames.SimpleFileBrowser.Scripts;
using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject FileBrowserPrefab;

    public InputField pathField;
    public Button loadButton;
    public CanvasGroup mainCanvas;

    public ModalMenuController storedGameLoaderMenu;
    public StoredGameRemover storedGameRemover;

    private StatusText statusText;

    private event EventHandler<EventArgs> OnLoadingProcessFinished;
    private event EventHandler<ExceptionEventArgs> OnLoadingProcessException;

    private bool isLoadFinished;

    public MainMenuController()
    {
    }

    private void Start()
    {
        ObjectManager.CurrentGEM = new GameElementManager("emptyGEM");
        PersistanceHelper.PersistentDataPath = Application.persistentDataPath;
        OnLoadingProcessFinished += new EventHandler<EventArgs>(LoadGameMenuScene);
        OnLoadingProcessException += new EventHandler<ExceptionEventArgs>(HandleLoadingProcessException);
        isLoadFinished = false;
        statusText = FindObjectOfType<StatusText>();
        InitStoredGameLoaderMenu();
    }

    private void Update()
    {
        if(pathField.text == null || pathField.text.Equals(""))
        {
            loadButton.interactable = false;
        } else
        {
            loadButton.interactable = true;
        }
        if (isLoadFinished)
        {
            isLoadFinished = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            SceneManager.LoadScene("GameMenu_Android");
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
            SceneManager.LoadScene("GameMenu_Standalone");
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
             Application.Quit();
        }
#endif
    }

    private void InitStoredGameLoaderMenu()
    {
        storedGameLoaderMenu.menuName.text = "Load stored game";
        List<string> storedGameNames = PersistanceHelper.GetStoredGameNames();
        foreach(string storedGameName in storedGameNames)
        {
            storedGameLoaderMenu.AddButton(storedGameName, delegate { LoadStoredGame(storedGameName); });
        }
        storedGameLoaderMenu.AddBackButton();
    }

    private void LoadGameMenuScene(object sender, EventArgs e)
    {
        Debug.Log("Descriptor loading is finished!");
        isLoadFinished = true;
    }

    private void HandleLoadingProcessException(object sender, ExceptionEventArgs args)
    {
        Debug.LogError(args.Msg + "\\n" + args.Exc, this);
        mainCanvas.interactable = true;
        statusText.SetStatus("An error occured during loading!", 2f);
    }


    public void LoadDescriptor()
    {
        statusText.SetStatus("Loading from descriptor. Please wait...");
        mainCanvas.interactable = false;
        //DescriptorLoaderUtility.LoadDescriptor(pathField.text, OnLoadingProcessFinished, OnLoadingProcessException);
        Thread loadingThread = new Thread(() => DescriptorLoaderUtility.LoadDescriptor(pathField.text, OnLoadingProcessFinished, OnLoadingProcessException));
        loadingThread.Start();
    }

    private void LoadStoredGame(string gameName)
    {
        statusText.SetStatus("Loading game. Please wait...");
        mainCanvas.interactable = false;
        Thread loadingThread = new Thread(() => StartStoredGameLoading(gameName, OnLoadingProcessFinished, OnLoadingProcessException));
        loadingThread.Start();
    }

    private void StartStoredGameLoading(string gameName, EventHandler<EventArgs> OnFinished, EventHandler<MainMenuController.ExceptionEventArgs> OnError)
    {
        try
        {
            new DescriptorProcessor().SetExistingGemAsCurrent(gameName);
            OnFinished(null, EventArgs.Empty);
        }
        catch (System.Exception e)
        {
            OnError(null, new MainMenuController.ExceptionEventArgs("Error occoured during the loading of the stored game: " + gameName + "!", e));
        }
    }

    public void OpenLoadStoredGamesMenu()
    {
        RefreshStoredGamesMenu();
        storedGameLoaderMenu.OpenMenu(false);
    }

    private void RefreshStoredGamesMenu()
    {
        List<string> storedGameNames = PersistanceHelper.GetStoredGameNames();
        storedGameLoaderMenu.KeepButtons(storedGameNames);
    }

    public void OpenManageStoredGamesMenu()
    {
        storedGameRemover.OpenMenu();
    }

    //Reading from Application.streamingAssetsPath.
    public void LoadTestGame()
    {
        StartCoroutine(DescriptorLoaderUtility.LoadTestDescriptor("AwesomeTestGame.zip", OnLoadingProcessFinished, OnLoadingProcessException));
    }
    

    public void BrowseFiles()
    {

#if UNITY_ANDROID //&& !UNITY_EDITOR
        // Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";
        // Set the mode to save or load
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
        
        fileBrowserScript.OpenFilePanel(new string[] { "zip"});
        // Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
        fileBrowserScript.OnFileSelect += SetFilePath;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        var extensions = new[] {
            new ExtensionFilter("Zip Files", "zip"),
            new ExtensionFilter("All Files", "*" ),
        };
        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => { if (paths.Length != 0) { SetFilePath(paths[0]); } });
#endif

    }

    private void SetFilePath(string path)
    {
        if (path.Length != 0)
        {
            pathField.text = path;
        }
        else
        {
            Debug.Log("Invalid path given");
        }
    }


    private void RaiseLoadingProcessException(String msg, Exception e)
    {
        if(OnLoadingProcessException != null)
        {
            OnLoadingProcessException(null, new ExceptionEventArgs(msg, e));
        }
    }

    public class ExceptionEventArgs : EventArgs
    {
        private string msg;
        private Exception exception;

        public ExceptionEventArgs(string msg, Exception exception)
        {
            this.msg = msg;
            this.exception = exception;
        }

        public string Msg
        {
            get
            {
                return msg;
            }
        }

        public Exception Exc
        {
            get
            {
                return exception;
            }
        }
    }
}
