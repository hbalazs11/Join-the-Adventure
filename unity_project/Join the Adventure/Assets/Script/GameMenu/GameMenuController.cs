﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

    public Text gameNameTextUI;
    public Text greetingTextUI;

    public ModalMenuController langMenuController;
    
    private ILogger logger;


    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        logger = Injector.Logger;
        LoadTexts();
        InitLangMenu();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void StartGame()
    {
        logger.LogInfo("Kezdődik a játék, juhú");
#if UNITY_ANDROID && !UNITY_EDITOR
            SceneManager.LoadScene("Game_Android");
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        SceneManager.LoadScene("Game_Standalone");
#endif
    }

    private void LoadTexts()
    {
        gameNameTextUI.text = Injector.GameElementManager.GameProperties.GameNameText.GetText();
        greetingTextUI.text = Injector.GameElementManager.GameProperties.GreetingText.GetText();
    }

    private void InitLangMenu()
    {
        langMenuController.SetMenuName("Languages");
        foreach (string lang in Injector.GameElementManager.AvailableLangs)
        {
            langMenuController.AddButton(lang, delegate { ChangeLang(lang); });
        }
        //langMenuController.AddBackButton();
        langMenuController.SetActive(false);
    }

    private void ChangeLang(string lang)
    {
        Injector.GameElementManager.CurrentLang = lang;
        LoadTexts();
        ChangeLangMenuActivation();
    }

    public void ChangeLangMenuActivation()
    {
        langMenuController.ChangeActivation();
    }
}
