using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameElementManager elementManager;
    private ILogger logger;

    private Description desctiptionScript;
    private MenuController menuController;

    void Awake()
    {
        elementManager = Injector.GameElementManager;
        logger = Injector.Logger;

        desctiptionScript = FindObjectOfType<Description>();
        menuController = FindObjectOfType<MenuController>();
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
    }

    public void LoadRoom(GERoom room)
    {
        elementManager.CurrentRoom = room;
        LoadCurrentRoom();
    }
}
