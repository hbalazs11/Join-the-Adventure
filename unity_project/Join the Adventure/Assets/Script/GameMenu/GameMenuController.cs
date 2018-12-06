using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

    public Text gameNameTextUI;
    public Text greetingTextUI;

    public ModalMenuController langMenuController;
    
    private ILogger logger;
    private BcgImage bcgImageScript;
    private byte[] imageBytes;


    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        logger = ObjectManager.Logger;
        LoadTexts();
        InitLangMenu();
        bcgImageScript = GetComponent<BcgImage>();
        GameElementManager gem = ObjectManager.CurrentGEM;
        Thread loadingThread = new Thread(() => StartLoadBcgImage(gem.GameStorageName, gem.GameProperties.MenuImgSrc));
        loadingThread.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if(imageBytes != null)
        {
            bcgImageScript.SetImage("", imageBytes);
            imageBytes = null;
        }
	}

    private void StartLoadBcgImage(string gameStoreName, string imgName)
    {
        imageBytes = PersistanceHelper.GetImage(gameStoreName, imgName);
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
        gameNameTextUI.text = System.Text.RegularExpressions.Regex.Unescape( ObjectManager.CurrentGEM.GameProperties.GameNameText.GetText());
        greetingTextUI.text = System.Text.RegularExpressions.Regex.Unescape( ObjectManager.CurrentGEM.GameProperties.GreetingText.GetText());
    }

    private void InitLangMenu()
    {
        langMenuController.SetMenuName("Languages");
        foreach (string lang in ObjectManager.CurrentGEM.AvailableLangs)
        {
            langMenuController.AddButton(lang, delegate { ChangeLang(lang); });
        }
        //langMenuController.AddBackButton();
        langMenuController.SetActive(false);
    }

    private void ChangeLang(string lang)
    {
        ObjectManager.CurrentGEM.CurrentLang = lang;
        LoadTexts();
        ChangeLangMenuActivation();
    }

    public void ChangeLangMenuActivation()
    {
        langMenuController.ChangeActivation();
    }
}
