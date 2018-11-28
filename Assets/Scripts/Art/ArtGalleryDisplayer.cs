using UnityEngine.UI;
using UnityEngine;

public class ArtGalleryDisplayer : MonoBehaviour {
    public Image image;
    public AspectRatioFitter aspect;
    public GameObject lockedPrompt;
    private Artpiece _art;
    public void ChangeImage(Artpiece art) {
        Sprite sprite = art.Image;
        image.sprite = sprite;
        image.color = art.Owned ? Color.white : new Color(0.1f, 0.1f, 0.1f, 1);
        lockedPrompt.SetActive(!art.Owned);
        aspect.aspectRatio = sprite.bounds.size.x / sprite.bounds.size.y;
        _art = art;
        return;
    }

    public void ShowArtpieceDetail()
    {
        if (_art == null || !_art.Owned) return;
        ArtpieceDisplayer displayer = GameObject.FindWithTag("ArtpieceDisplayer").GetComponent<ArtpieceDisplayer>();
        if(displayer != null) {
            displayer.DisplayArtpiece(_art,false, null);
        }
    }
}
