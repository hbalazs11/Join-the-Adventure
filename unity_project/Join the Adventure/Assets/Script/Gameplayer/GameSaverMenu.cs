using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ModalMenuController))]
public class GameSaverMenu : MonoBehaviour {

    public InputField inputField;
    public Text inputPlaceholder;
    public Button saveButton;
    public GameSaver gameSaver;

    private ModalMenuController menuController;
    private GameElementManager elementManager;
    
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenMenu(GameElementManager gem, bool fromParent)
    {
        if (elementManager == null || elementManager != gem)
        {
            InitMenu(gem);
        } else {
            RefreshNames();
        }
        inputField.text = "";
        menuController.OpenMenu(fromParent);
    }

    private void RefreshNames()
    {
        List<string> currentNames = menuController.GetContainedButtonNames();
        foreach (string saveName in elementManager.savedGameNames)
        {
            if (!currentNames.Contains(saveName))
            {
                menuController.AddButton(saveName, delegate { SetSaveName(saveName); });
            }
        }
    }

    private void InitMenu(GameElementManager gem)
    {
        elementManager = gem;
        menuController = this.gameObject.GetComponent<ModalMenuController>();
        saveButton.onClick.AddListener(SaveGame);
        saveButton.GetComponentInChildren<Text>().text = LabelUtility.Instance.GetLabel(LabelNames.SAVE);
        menuController.SetMenuName(LabelUtility.Instance.GetLabel(LabelNames.SAVE));
        inputPlaceholder.text = LabelUtility.Instance.GetLabel(LabelNames.SAVENAME);
        foreach (string savedGameName in Injector.GameElementManager.savedGameNames)
        {
            menuController.AddButton(savedGameName, delegate { SetSaveName(savedGameName); });
        }
    }

    private void SetSaveName(string name)
    {
        inputField.text = name;
    }

    private void SaveGame()
    {
        if (inputField.text.Length == 0) return;
        gameSaver.SaveGame(inputField.text);
        menuController.CloseMenu();
    }

}
