using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BcgImage : MonoBehaviour {

    public Image image;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetImage(string name, byte[] imgBytes)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imgBytes);
        //image.sprite = Sprite.Create(tex, image.rectTransform.rect, image.rectTransform.pivot);
        image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
