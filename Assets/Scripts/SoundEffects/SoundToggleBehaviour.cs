using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleBehaviour : MonoBehaviour {

    public Toggle musicToggle, soundToggle;
	void Start () {
        if(musicToggle == null)
        {
            Debug.Log("Music toggle: Missing");
        }
        musicToggle.isOn = MainMenuManager.Instance.MusicOn;

        if (soundToggle == null)
        {
            Debug.Log("Sound toggle: Missing");
        }
        soundToggle.isOn = MainMenuManager.Instance.SoundFXOn;
    }
}
