using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager> {
    protected MainMenuManager () {}
    bool _musicOn;
    const string playerPrefMusicOnKey = "MusicOn";
    const string playerPrefSoundFXOnKey = "SoundFXOn";
    public bool MusicOn {
        get { return _musicOn; }
        set 
        {
            if(bgmSource != null)
            {
                if (!_musicOn && value)
                {
                    bgmSource.Play();
                }
                if(_musicOn && !value)
                {
                    bgmSource.Stop();
                }
            }

            _musicOn = value;
            PlayerPrefs.SetInt(playerPrefMusicOnKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }


    bool _soundfxOn;

    public bool SoundFXOn
    {
        get { return _soundfxOn; }
        set
        {
            _soundfxOn = value;
            PlayerPrefs.SetInt(playerPrefSoundFXOnKey, value ? 1 : 0);
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
        if(PlayerPrefs.HasKey(playerPrefMusicOnKey))
        {
            _musicOn = PlayerPrefs.GetInt(playerPrefMusicOnKey) == 1;
        }
        else {
            PlayerPrefs.SetInt(playerPrefMusicOnKey, 1);
            _musicOn = true;
        }

        if (PlayerPrefs.HasKey(playerPrefSoundFXOnKey))
        {
            _soundfxOn = PlayerPrefs.GetInt(playerPrefSoundFXOnKey) == 1;
        }
        else
        {
            PlayerPrefs.SetInt(playerPrefSoundFXOnKey, 1);
            _soundfxOn = true;
        }

        if (bgmSource == null) {
            Debug.Log("Missing BGM AudioSource");
        } else if (_musicOn)
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
