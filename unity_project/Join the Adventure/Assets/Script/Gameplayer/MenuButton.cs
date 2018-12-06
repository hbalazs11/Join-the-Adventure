using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text buttonText;
    private Button button;


    void Awake()
    {
        button = this.GetComponent<Button>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetText(string buttonText)
    {
        this.buttonText.text = buttonText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public Button.ButtonClickedEvent onClick 
    {
        get
        {
            return button.onClick;
        }
        set
        {
            button.onClick = value;
        }
    }
}
