using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager> {

    protected GameManager() {}

    public void QuitToMenu () 
    {
        SceneManager.LoadScene(0);
    }


}
