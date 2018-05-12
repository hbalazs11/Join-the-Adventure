using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

    public Text gameNameTextUI;
    public Text greetingTextUI;

    private GameElementManager elementManager;
    private ILogger logger;

    void Awake()
    {
        elementManager = Injector.GameElementManager;
        logger = Injector.Logger;
    }

    // Use this for initialization
    void Start () {
        gameNameTextUI.text = elementManager.GameProperties.GameNameText.GetText();
        greetingTextUI.text = elementManager.GameProperties.GreetingText.GetText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        logger.LogInfo("Kezdődik a játék, juhú");
        SceneManager.LoadScene("Game");
    }
}
