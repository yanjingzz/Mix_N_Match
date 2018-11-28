using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour {
    public List<Artpiece> allArt;
    public GameObject ArtUIPrefab;
    public GameObject layout;
    public GameObject content;

    private void Start()
    {
        foreach (Artpiece art in allArt) 
        {
            var artGO = Instantiate(ArtUIPrefab, layout.transform);
            var displayer = artGO.GetComponent<ArtGalleryDisplayer>();
            displayer.ChangeImage(art);
            
        }
    }
}
