using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour {

    private const float SCROLLSIZE = 1.0f;

    public Text roomName;
    public Text descriptionText;
    public Scrollbar VertivalScrollBar;

    private const string BR = "\n";
    private bool isDescTop = true;

    public static Description GetInstance()
    {
        return FindObjectOfType<Description>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isDescTop && VertivalScrollBar.size < SCROLLSIZE)
        {
            PinDescriptionToBottom();
            isDescTop = false;
        } 
        if (!isDescTop && VertivalScrollBar.size > 0.95f)
        {
            PinDescriptionToTop();
            isDescTop = true;
        }
	}

    private void PinDescriptionToTop()
    {
        RectTransform mRectTransform = descriptionText.transform as RectTransform;
        mRectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        mRectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        mRectTransform.pivot = new Vector2(0.5f, 1.0f);
    }

    private void PinDescriptionToBottom()
    {
        RectTransform mRectTransform = descriptionText.transform as RectTransform;
        mRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        mRectTransform.anchorMax = new Vector2(1.0f, 0.0f);
        mRectTransform.pivot = new Vector2(0.5f, 0.0f);
    }

    private string ConvertText(string baseTxt)
    {
        return System.Text.RegularExpressions.Regex.Unescape(baseTxt);
    }

    public void SetRoomName(string roomName)
    {
        this.roomName.text = roomName;
    }

    public void ClearDescription()
    {
        this.descriptionText.text = "";
    }

    public void AddDescriptionText(string text)
    {
        this.descriptionText.text += ConvertText(BR + BR + text);
    }

    public void SetDescriptionText(string text)
    {
        this.descriptionText.text = ConvertText(text);
    }
}
