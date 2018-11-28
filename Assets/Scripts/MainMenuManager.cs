using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager> {
    protected MainMenuManager () {}
    bool _soundOn;
    const string playerPrefSoundOnKey = "SoundOn";
    public bool SoundOn {
        get { return _soundOn; }
        set 
        {
            if(bgmSource != null)
            {
                if (!_soundOn && value)
                {
                    bgmSource.Play();
                }
                if(_soundOn && !value)
                {
                    bgmSource.Stop();
                }
            }

            _soundOn = value;
            PlayerPrefs.SetInt(playerPrefSoundOnKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    AudioSource bgmSource;
    
    private void Start()
    {
        DontDestroyOnLoad(this);
        GameObject bgm = Resources.Load<GameObject>("prefabs/BGM");
        var bgmInstance = Instantiate(bgm);
        DontDestroyOnLoad(bgmInstance);
        bgmSource = bgmInstance.GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey(playerPrefSoundOnKey))
        {
            _soundOn = PlayerPrefs.GetInt(playerPrefSoundOnKey) == 1;
        }
        else {
            PlayerPrefs.SetInt(playerPrefSoundOnKey, 1);
            _soundOn = true;
        }
        if(bgmSource == null) {
            Debug.Log("Missing BGM AudioSource");
        } else if (_soundOn)
        {

            bgmSource.Play();
        }
    }
    public void PlayGame() {
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
