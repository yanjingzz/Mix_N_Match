﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainbowManager : MonoBehaviour, IPlaceable {

    public static RainbowManager Instance { get; private set; }
    public DragNDropBehaviour dragNDropBehaviour;
    public bool IsFullAtStart;

    public GameObject fullRainbow;
    public GameObject RainbowParticles;

    public GameObject yellowPiece, orangePiece, redPiece, greenPiece, bluePiece, violetPiece;


    private SpriteRenderer rainbowRenderer;


    bool hasYellow, hasOrange, hasRed, hasGreen, hasBlue, hasViolet;
    bool hasFull
    {
        get
        {
            return hasYellow && hasOrange && hasRed && hasGreen && hasBlue && hasViolet;
        }
        set
        {
            hasRed = hasOrange = hasYellow = hasGreen = hasBlue = hasViolet = value;
        }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        if (dragNDropBehaviour != null) 
        {
            dragNDropBehaviour.placeable = this;
        } else {
            Debug.LogWarning("RainbowManager: DragNDropBehaviour not found");
        }
            
    }

    private void Start()
    {
        if (IsFullAtStart) 
        {
            hasFull = true;
            fullRainbow.SetActive(true);
        }
        rainbowRenderer = fullRainbow.GetComponent<SpriteRenderer>();
        if(rainbowRenderer == null) {
            Debug.LogWarning("RainbowManager: Rainbow renderer not found");
        }
    }




    public void Matched (Piece.Paint paint) {
        if (paint == Piece.Paint.RedBig || paint == Piece.Paint.Vermillion) 
        {
            hasRed = true;
            redPiece.SetActive(true);
        }
            
        if (paint == Piece.Paint.OrangeBig || paint == Piece.Paint.Amber)
        {
            hasOrange = true;
            orangePiece.SetActive(true);
        }
        if (paint == Piece.Paint.YellowBig || paint == Piece.Paint.Lime)
        {
            hasYellow = true;
            yellowPiece.SetActive(true);
        }
        if (paint == Piece.Paint.GreenBig || paint == Piece.Paint.Turquoise)
        {
            hasGreen = true;
            greenPiece.SetActive(true);
        }
        if (paint == Piece.Paint.BlueBig || paint == Piece.Paint.Indigo)
        {
            hasBlue = true;
            bluePiece.SetActive(true);
        }
        if (paint == Piece.Paint.VioletBig || paint == Piece.Paint.Magenta)
        {
            hasViolet = true;
            violetPiece.SetActive(true);
        }
        if (hasFull) {
            redPiece.SetActive(false);
            orangePiece.SetActive(false);
            yellowPiece.SetActive(false);
            greenPiece.SetActive(false);
            bluePiece.SetActive(false);
            violetPiece.SetActive(false);

            fullRainbow.SetActive(true);
        }
    }

    public void Place()
    {
        Debug.Log("Placing rainbow");
        BoardManager board = BoardManager.Instance;
        var tf = fullRainbow.transform;
        var hex = board.HexAtPoint(tf.position, board.center);
        if (!board.IsOnBoard(tf.position) || board[hex] == Piece.Paint.Empty)
        {
            tf.DOLocalMove(Vector3.zero, 0.2f);
            return;
        }

        board[hex] = Piece.Paint.Empty;
        foreach (var dir in Piece.Hex.directions)
        {
            board[hex.neighbour(dir)] = Piece.Paint.Empty;
        }
        var particles = Instantiate(RainbowParticles, tf, false);
        Destroy(particles, 3f);

        rainbowRenderer.DOFade(0f, 0.3f).OnComplete(() => 
        { 
            fullRainbow.SetActive(false); 
            tf.localPosition = Vector3.zero;
            rainbowRenderer.color = Color.white;
        });
        ScoreManager.Instance.Placed();
        hasFull = false;
    }
}
