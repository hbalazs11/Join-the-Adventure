using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text gameOverTextLabel;
    public CanvasGroup gameMainMenuPanel;
    public CanvasGroup menuCanvas;

    private GameElementManager elementManager;
    private ILogger logger;

    private Description desctiptionScript;
    private MenuController menuController;
    private BcgImage bcgImageScript;

    void Awake()
    {
        elementManager = Injector.GameElementManager;
        logger = Injector.Logger;

        desctiptionScript = FindObjectOfType<Description>();
        menuController = FindObjectOfType<MenuController>();
        bcgImageScript = FindObjectOfType<BcgImage>();
    }

    // Use this for initialization
    void Start () {
        LoadCurrentRoom();
	}

    public static GameController GetInstance()
    {
        return FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadCurrentRoom()
    {
        desctiptionScript.SetRoomName(elementManager.CurrentRoom.NameText.GetText());
        desctiptionScript.SetDescriptionText(elementManager.CurrentRoom.DescText.GetText());
        menuController.LoadRoom(elementManager.CurrentRoom);
        string imgName = elementManager.CurrentRoom.ImgPath;
        bcgImageScript.SetImage(imgName, elementManager.ImgResources[imgName].ToArray());


    }

    public void LoadRoom(GERoom room)
    {
        elementManager.CurrentRoom = room;
        LoadCurrentRoom();
    }

    public void ShowGameOver(string gameOverTxt)
    {
        gameOverTextLabel.text = gameOverTxt;
        gameOverTextLabel.gameObject.SetActive(true);
        desctiptionScript.SetRoomName("");
    }

    public void HeadBackToGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void GameMMOpenMenu()
    {
        if (gameMainMenuPanel.gameObject.activeSelf)
        {
            GameMMBack();
        }
        else
        {
            gameMainMenuPanel.gameObject.SetActive(true);
            menuCanvas.interactable = false;
        }
    }

    public void GameMMSave()
    {

    }

    public void GameMMLoad()
    {

    }

    public void GameMMExit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameMMBack()
    {
        gameMainMenuPanel.gameObject.SetActive(false);
        menuCanvas.interactable = true;
    }
}
