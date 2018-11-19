using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtpieceDisplayer : MonoBehaviour {
    public GameObject panel;
    public Text title;
    public Text description;
    public Image image;
    Action _disableCallback;

    public void DisplayArtpiece(Artpiece artpiece, Action disableCallback)
    {
        if (artpiece != null)
        {
            title.text = artpiece.Title;
            description.text = artpiece.Description;
            image.sprite = artpiece.Image;
        } else {
            Debug.LogError("Artpiece displayer: missing displayed artpiece");
        }
        panel.SetActive(true);
        _disableCallback = disableCallback;
    }

    public void ClosePanel()
    {
        if (_disableCallback != null)
            _disableCallback();
        _disableCallback = null;
        panel.SetActive(false);
    }



}
