using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject Loading;

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
