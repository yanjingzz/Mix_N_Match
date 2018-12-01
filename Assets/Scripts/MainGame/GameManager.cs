using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager> {

    protected GameManager() {}
    HashSet<Paint> encounteredPaint = new HashSet<Paint>();
    public GameObject GameoverPanel;
    public Text scoreRecap;
    public RainbowManager rainbow;
    private string ScoreRemark(int score) 
    {
        if(score <= 1000) 
        {
            return "Keep at it! You'll get\n the hang of it in no time!";
        }
        if (score <= 4000)
        {
            return "Good job raising the monsters!";
        }
        if (score <= 7000)
        {
            return "You're a paint master!";
        }
        if (score <= 10000)
        {
            return "Wow! Color me impressed!";
        }
        return "How did you even get a highscore like this!";

    }

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
    public void GameOver()
    {
        if(!rainbow.HasFull)
        {
            int score = ScoreManager.Instance.Score;
            string scoreText = "Your score: " + score + "\n" + ScoreRemark(score);
            scoreRecap.text = scoreText;
            GameoverPanel.SetActive(true);
        }

        
    }
    public void Restart()
    {
        MainMenuManager.Instance.PlayGame();
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
