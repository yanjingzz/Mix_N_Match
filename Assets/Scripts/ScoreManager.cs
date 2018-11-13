using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> {


    protected ScoreManager () {}
    public Text scoreText;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            EventManager.Instance.OnUpdatedScore(value);

            UpdateScoreText();

        }
    }
    private int _score;




    private void Start()
    {
        EventManager.Instance.OnMatched += Matched;
        EventManager.Instance.OnPlaced += Placed;
        EventManager.Instance.OnBombed += Bombed;
    }


    private void UpdateScoreText()
    {
        scoreText.text = Score.ToString();
    }

    public void Matched(Paint paint, int matches)
    {
        if (matches == 0) return;
        Score += Math.Max(1, matches - 2) * 100;

    }

    public void Placed()
    {
        Score += 10;
    }

    public void Bombed()
    {
        Score += 500;
    }
}
