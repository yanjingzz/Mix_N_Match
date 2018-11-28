using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager> {

    protected GameManager() {}
    HashSet<Paint> encounteredPaint = new HashSet<Paint>();

    private void Start()
    {
        encounteredPaint.Add(Paint.RedBig);
        encounteredPaint.Add(Paint.RedSmall);
        encounteredPaint.Add(Paint.YellowBig);
        encounteredPaint.Add(Paint.YellowSmall); 
        encounteredPaint.Add(Paint.BlueBig); 
        encounteredPaint.Add(Paint.BlueSmall);
        EventManager.Instance.OnEncounter += EncounterColor;
        
    }

    public void QuitToMenu () 
    {
        SceneManager.LoadScene(0);
    }

    public void EncounterColor(Paint paint) 
    {
		encounteredPaint.Add(paint);
    }

    bool _hasEncountered(Paint paint)
    {
        return encounteredPaint.Contains(paint);
    }

    public static bool HasEncountered(Paint paint) {
        return Instance._hasEncountered(paint);
    }


}
