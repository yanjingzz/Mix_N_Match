using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance { get; private set; }

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
            if (_score >= 600)
            {
                Spawner.Instance.SecondaryEnabled = true;
            }
            UpdateScoreText();

        }
    }
    private int _score;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }



    private void UpdateScoreText()
    {
        scoreText.text = Score.ToString();
    }

    public void Matched(int matches)
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
