using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour {

    private Text text;


	void Start () {
        text = GetComponent<Text>();
        text.enabled = false;
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
        text.enabled = true;
    }

    public void SetStatus(string msg, float duration)
    {
        SetText(msg);
        text.enabled = true;
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
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        text.enabled = false;
        text.color = originalColor;
    }
}
