using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SoundToggleBehaviour : MonoBehaviour {

    Toggle _toggle;
	void Start () {
        _toggle = GetComponent<Toggle>();
        if(_toggle == null)
        {
            Debug.Log("Sound toggle: Missing");
        }
        _toggle.isOn = MainMenuManager.Instance.SoundOn;
	}
}
