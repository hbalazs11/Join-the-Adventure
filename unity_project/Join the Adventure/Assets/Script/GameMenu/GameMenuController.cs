using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

    public Text gameNameTextUI;
    public Text greetingTextUI;

    public ModalMenuController langMenuController;

    private GameElementManager elementManager;
    private ILogger logger;

    void Awake()
    {
        elementManager = Injector.GameElementManager;
        logger = Injector.Logger;
    }

    // Use this for initialization
    void Start () {
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
        gameNameTextUI.text = elementManager.GameProperties.GameNameText.GetText();
        greetingTextUI.text = elementManager.GameProperties.GreetingText.GetText();
    }

    private void InitLangMenu()
    {
        langMenuController.SetMenuName("Languages");
        foreach (string lang in elementManager.AvailableLangs)
        {
            langMenuController.AddButton(lang, delegate { ChangeLang(lang); });
        }
        //langMenuController.AddBackButton();
        langMenuController.SetActive(false);
    }

    private void ChangeLang(string lang)
    {
        elementManager.CurrentLang = lang;
        LoadTexts();
        ChangeLangMenuActivation();
    }

    public void ChangeLangMenuActivation()
    {
        langMenuController.ChangeActivation();
    }
}
