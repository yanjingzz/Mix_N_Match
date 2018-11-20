using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager> {
    protected MainMenuManager () {}

    public GameObject MainMenu;
    public GameObject Loading;
    public GameObject BGMPrefab;
    public bool soundOn;

    GameObject bgmInstance;
    
    private void Start()
    {
        if(!ReferenceEquals(this, MainMenuManager.Instance)) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(this);
            bgmInstance = Instantiate(BGMPrefab);
            DontDestroyOnLoad(bgmInstance);
            if(soundOn)
            {
                bgmInstance.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void PlayGame() {
        MainMenu.SetActive(false);
        Loading.SetActive(true);
        StartCoroutine(AsyncLoadScene());
    }

    IEnumerator AsyncLoadScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }



}
