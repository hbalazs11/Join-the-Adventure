using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text gameOverTextLabel;
    public ModalMenuController escapeMenuController;
    public GameSaverMenu saverMenu;
    
    //private ILogger logger;

    private Description desctiptionScript;
    private MenuController menuController;
    private BcgImage bcgImageScript;
    private SavedGameLoader savedGameLoader;

    void Awake()
    {
        //logger = Injector.Logger;

        desctiptionScript = FindObjectOfType<Description>();
        menuController = FindObjectOfType<MenuController>();
        bcgImageScript = FindObjectOfType<BcgImage>();
        savedGameLoader = FindObjectOfType<SavedGameLoader>();
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
        if (bcgImgArgs != null)
        {
            bcgImageScript.SetImage(bcgImgArgs.imgName, bcgImgArgs.bytes);
            bcgImgArgs = null;
        }
    }

    private void InitEscMenu()
    {
        escapeMenuController.SetMenuName(LabelUtility.Instance.GetLabel(LabelNames.MENU));
        if (Injector.GameElementManager.GameProperties.IsMenuSaveAvailable)
        {
            escapeMenuController.AddButton(LabelUtility.Instance.GetLabel(LabelNames.SAVE), delegate { saverMenu.OpenMenu(Injector.GameElementManager); });
        }
        escapeMenuController.AddButton(LabelUtility.Instance.GetLabel(LabelNames.LOAD), savedGameLoader.OpenLoadMenuActivation);
        escapeMenuController.AddButton(LabelUtility.Instance.GetLabel(LabelNames.EXIT), GameMMExit);
        escapeMenuController.AddBackButton(LabelUtility.Instance.GetLabel(LabelNames.BACK));
        escapeMenuController.SetActive(false);
    }

    public void LoadCurrentRoom()
    {
        GameElementManager elementManager = Injector.GameElementManager;
        desctiptionScript.SetRoomName(elementManager.CurrentRoom.NameText.GetText());
        desctiptionScript.SetDescriptionText(elementManager.CurrentRoom.DescText.GetText());
        menuController.LoadRoom(elementManager.CurrentRoom);
        string imgName = elementManager.CurrentRoom.ImgPath;
        //bcgImageScript.SetImage(imgName, elementManager.ImgResources[imgName].ToArray());

        OnBcgImageLoadFinished += new EventHandler<ImgLoaderArgs>(LoadBcgImage);
        Thread loadingThread = new Thread(() => StartLoadBcgImage(elementManager.GameStorageName, imgName, OnBcgImageLoadFinished));
        loadingThread.Start();
    }
    public class ImgLoaderArgs : EventArgs
    {
        public string imgName;
        public byte[] bytes;
    }
    private event EventHandler<ImgLoaderArgs> OnBcgImageLoadFinished;
    private void StartLoadBcgImage(string gameStoreName, string imgName, EventHandler<ImgLoaderArgs> OnFinished)
    {
        ImgLoaderArgs args = new ImgLoaderArgs
        {
            imgName = imgName,
            bytes = PersistanceHelper.GetImage(gameStoreName, imgName)
        };
        OnFinished(null, args);
    }
    private ImgLoaderArgs bcgImgArgs;


    private void LoadBcgImage(object sender, ImgLoaderArgs e )
    {
        //bcgImageScript.SetImage(e.imgName, e.bytes);
        bcgImgArgs = e;
    }

    

    public void LoadRoom(GERoom room)
    {
        Injector.GameElementManager.CurrentRoom = room;
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
        Injector.GameElementManager = GameElementManager.GetInitialGEM();
#if UNITY_ANDROID //&& !UNITY_EDITOR
        SceneManager.LoadScene("GameMenu_Android");
#endif
#if UNITY_STANDALONE //|| UNITY_EDITOR
        SceneManager.LoadScene("GameMenu_Standalone");
#endif
    }

    public void GameMMOpenMenu()
    {
        escapeMenuController.OpenMenu();
    }

    private void GameMMSave()
    {

    }

    private void GameMMExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
