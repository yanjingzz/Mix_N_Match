using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject Loading;
    private MainMenuManager mainMenuManager;
    public bool MusicOn {
        set {
            mainMenuManager.MusicOn = value;
        }
        get {
            return mainMenuManager.MusicOn;
        }
    }
    public bool SoundFXOn
    {
        set
        {
            mainMenuManager.SoundFXOn = value;
        }
        get
        {
            return mainMenuManager.SoundFXOn;
        }
    }
    void Awake()
    {
        mainMenuManager = MainMenuManager.Instance;
    }
    public void PlayGame() 
    {
        if(MainMenu != null && Loading != null)
        {
            MainMenu.SetActive(false);
            Loading.SetActive(true);
            mainMenuManager.PlayGame();
        }

    }
}
