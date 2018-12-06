using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour {

    private Text text;
    public Image background;


	void Start () {
        text = GetComponent<Text>();
        SetTextEnable(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetText(string msg)
    {
        text.text = msg;
    }

    public void SetStatus(string msg)
    {
        SetText(msg);
        SetTextEnable(true);
    }

    public void SetStatus(string msg, float duration)
    {
        SetText(msg);
        SetTextEnable(true);
        StartCoroutine(RemoveStatusAfterTime(duration));
    }

    IEnumerator RemoveStatusAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        RemoveStatus();
    }

    public void RemoveStatus(float fadeOutTime = 1)
    {
        StartCoroutine(FadeOutRoutine(fadeOutTime));
    }

    private IEnumerator FadeOutRoutine(float fadeOutTime)
    {
        Color originalColor = text.color;
        Color originalBcgColor = new Color();
        if(background != null)
        {
            originalBcgColor = background.color;
        }
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            if(background != null)
            {
                background.color = Color.Lerp(originalBcgColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            }
            yield return null;
        }
        SetTextEnable(false);
        text.color = originalColor;
        if(background != null)
        {
            background.color = originalBcgColor;
        }
    }

    private void SetTextEnable(bool value)
    {
        if(background != null)
        {
            background.enabled = value;
        }
        text.enabled = value;
    }
}
