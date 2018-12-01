using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnPickup : MonoBehaviour {

    public AudioClip pickUpSound;

    private void OnMouseDown()
    {
        //Debug.Log("Playing pickup sound");
        if(MainMenuManager.Instance.SoundFXOn)
            AudioSource.PlayClipAtPoint(pickUpSound, Camera.main.transform.position);
    }
}
