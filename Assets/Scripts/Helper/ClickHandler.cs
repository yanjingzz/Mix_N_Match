using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject Loading;
    private MainMenuManager mainMenuManager;
    public bool SoundOn {
        set {
            MainMenuManager.Instance.SoundOn = value;
        }
        get {
            return MainMenuManager.Instance.SoundOn;
        }
    }
    private void Start()
    {
        mainMenuManager = MainMenuManager.Instance;
    }
    public void PlayGame() 
    {
        if(MainMenu != null && Loading != null)
        {
            MainMenu.SetActive(false);
            Loading.SetActive(true);
            MainMenuManager.Instance.PlayGame();
        }

    }
}
