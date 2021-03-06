﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PieceManager : MonoBehaviour, IPlaceable {

    Vector3 initialPos;
    List<PaintManager> paintManagers = new List<PaintManager>();
    // Use this for initialization

    public static List<Sprite> monsterSprites;
    public List<Sprite> previewSprites;
    #region MonoBehaviour Messages

    void Start () {
        initialPos = transform.position;
        DragNDropBehaviour dndb = GetComponent<DragNDropBehaviour>();
        if (dndb != null)
            dndb.placeable = this;
	}

    #endregion

    public void Place() 
    {
        BoardManager board = BoardManager.Instance;

        //check if it is legal to place piece
        foreach (PaintManager paint in paintManagers)
        {
            if (!board.IsLegalToPut(paint.Color, paint.transform.position))
            {
                transform.DOMove(initialPos, 0.3f);
                return;
            }
        }

        //place piece
        foreach (PaintManager paint in paintManagers)
        {
            ScoreManager.Instance.Placed();
            var pos = board.HexAtPoint(paint.transform.position, board.center);
            board[pos] = paint.Color + board[pos];
            if (!((Paint)board[pos]).IsSmall()) paint.Color = Paint.Empty;
            paint.transform.DOMove(board.CenterPosAtHex(pos, board.center), 0.2f);
        }

        //let board manager find matches
        ScoreManager.Instance.Matched(board.FindMatches());


        //spawn new piece
        Spawner.Instance.Spawn();

        //delete itself after one second
        Destroy(gameObject, 2f);

    }



    public GameObject paintPrefab;


    public GameObject CreateGO(Piece piece) {
        GameObject pieceGO = Instantiate(paintPrefab, transform);
        //setting color id
        var paintManager = pieceGO.GetComponent<PaintManager>();
        if (paintManager == null) {
            Debug.LogError("PieceManager: paint prefab has no color id");
        } else {
            paintManagers.Add(paintManager);
            paintManager.Color = piece.color;
        }
        // setting position
        pieceGO.transform.SetParent(transform);
        pieceGO.transform.localPosition = BoardManager.Instance.CenterPosAtHex(piece.hexPos, Vector2.zero);

        return pieceGO;
    }
}



