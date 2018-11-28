using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{

    protected EventManager() { }

    public Action OnPlaced  { get; set; }
    public Action<Paint,int> OnMatched { get; set; }
    public Action<int> OnUpdatedScore { get; set; }
    public Action OnBombed { get; set; }
    public Action<Paint> OnEncounter { get; set; }

    public void PlacePiece() {
        if(OnPlaced != null) {
            OnPlaced();
        }
    }

    public void MatchPaint(Paint paint, int matches)
    {
        if (OnMatched != null)
        {
            OnMatched(paint, matches);
        }
    }

    public void Bomb()
    {
        if (OnBombed != null)
        {
            OnBombed();
        }
    }

    public void UpdateScore(int score)
    {
        if (OnUpdatedScore != null)
        {
            OnUpdatedScore(score);
        }
    }

    public void EncounterColor(Paint paint)
    {
        if (OnEncounter != null)
        {
            OnEncounter(paint);
        }

    }


}
