using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffects : MonoBehaviour {

    public AudioClip dropSound;
    public AudioClip matchSound;
    public AudioClip bombSound;
    private void Start()
    {
        EventManager.Instance.OnPlaced += PlayPlaceSound;
        EventManager.Instance.OnMatched += PlayMatchSound;
        EventManager.Instance.OnBombed += PlayBombSound;
    }

    void PlayPlaceSound()
    {
        if(MainMenuManager.Instance.SoundFXOn)
            AudioSource.PlayClipAtPoint(dropSound, Camera.main.transform.position);
    }

    void PlayMatchSound(Paint _, int __)
    {
        if (MainMenuManager.Instance.SoundFXOn)
            AudioSource.PlayClipAtPoint(matchSound, Camera.main.transform.position);
    }

    void PlayBombSound()
    {
        if (MainMenuManager.Instance.SoundFXOn)
            AudioSource.PlayClipAtPoint(bombSound, Camera.main.transform.position);
    }

}
