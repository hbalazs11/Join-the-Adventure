using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text gameOverTextLabel;
    public ModalMenuController escapeMenuController;

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
        InitEscMenu();
	}

    public static GameController GetInstance()
    {
        return FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update () {

    }

    private void InitEscMenu()
    {
        escapeMenuController.SetMenuName("Menu");
        escapeMenuController.AddButton("Save", GameMMSave);
        escapeMenuController.AddButton("Load", GameMMLoad);
        escapeMenuController.AddButton("Exit", GameMMExit);
        escapeMenuController.AddBackButton();
        escapeMenuController.SetActive(false);
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
        escapeMenuController.OpenMenu();
    }

    private void GameMMSave()
    {

    }

    private void GameMMLoad()
    {

    }

    private void GameMMExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
