using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameSaver : MonoBehaviour {

    private bool isSaveInAction;
    private bool isSaveFinished;
    private readonly object syncLock = new object();
    private StatusText statusText;

    void Start () {
        isSaveInAction = false;
        isSaveFinished = false;
        statusText = FindObjectOfType<StatusText>();
    }
	
	void Update () {
        if (isSaveInAction)
        {
            if(statusText != null)
            {
                statusText.SetStatus("Saving...");
            }
        }
        if (isSaveFinished)
        {
            if (statusText != null)
            {
                statusText.SetStatus("Status is saved.", 2f);
            }
            isSaveFinished = false;
        }
	}

    public void SaveGame(string saveName)
    {
        Thread loadingThread = new Thread(() => StartSave(Injector.GameElementManager.GameStorageName, saveName, Injector.GameElementManager));
        isSaveInAction = true;
        loadingThread.Start();
    }
    public void AutoSave()
    {
        Thread loadingThread = new Thread(() => StartAutoSave(Injector.GameElementManager.GameStorageName, Injector.GameElementManager));
        isSaveInAction = true;
        loadingThread.Start();
    }

    public void SaveGameWithTimetag(string sourceName)
    {
        Thread loadingThread = new Thread(() => StartSaveWithTimetag(Injector.GameElementManager.GameStorageName, sourceName, Injector.GameElementManager));
        isSaveInAction = true;
        loadingThread.Start();
    }

    private void StartSaveWithTimetag(string gameName, string sourceName, GameElementManager gem)
    {
        string saveName = PersistanceHelper.StoreSaveStationSavedGameGEM(gameName, sourceName, gem);
        ManageAfterSave(saveName, gem);
    }

    private void StartAutoSave(string gameName, GameElementManager gem)
    {
        string saveName = PersistanceHelper.StoreAutoSavedGameGEM(gameName, gem);
        ManageAfterSave(saveName, gem);
    }

    private void StartSave(string gameName, string saveName, GameElementManager gem)
    {
        PersistanceHelper.StoreSavedGameGEM(gameName, saveName, gem);
        ManageAfterSave(saveName, gem);
    }

    private void ManageAfterSave(string saveName, GameElementManager gem)
    {
        isSaveInAction = false;
        isSaveFinished = true;
        lock (syncLock)
        {
            if (!gem.savedGameNames.Contains(saveName))
            {
                gem.savedGameNames.Add(saveName);
            }
        }
    }
}
