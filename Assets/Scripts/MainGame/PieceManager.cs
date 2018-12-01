using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                paint.Flash();
                board.Flash(paint.transform.position);
                transform.DOMove(initialPos, 0.3f);
                return;
            }
        }

        //place piece
        foreach (PaintManager paint in paintManagers)
        {

            var hex = board.HexAtPoint(paint.transform.position, board.center);
            board[hex] = paint.Color + board[hex];
            if (!((Paint)board[hex]).IsSmall()) paint.Color = Paint.Empty;
            paint.transform.DOMove(board.CenterPosAtHex(hex, board.center), 0.2f);

        }


        EventManager.Instance.PlacePiece();
        //let board manager find matches
        //ScoreManager.Instance.Matched(board.FindMatches());


        //spawn new piece
        //Spawner.Instance.Spawn();

        //delete itself after one second
        Destroy(gameObject, 0.5f);

    }



    public GameObject paintPrefab;


    public void CreateGO(Piece piece) {
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

        return;
    }
    private Vector2 offset = Vector2.zero;
    public void CenterOnChildren() {
        var pos = Vector3.zero;
        int count = 0;
        foreach (Transform child in transform)
        {
            pos += child.localPosition;
            count++;
        }
        pos /= count;
        offset = pos;
        foreach (Transform child in transform)
        {
            child.localPosition -= pos;
        }

    }

    public bool CheckPlaceable()
    {
        BoardManager board = BoardManager.Instance;
        bool legalToPutSomewhere = false;
        //int count = 0;
        foreach(Hex hex in board.Positions)
        {
            bool legalToPutAtHex = true;
            foreach (PaintManager paint in paintManagers)
            {
                Vector2 localPos = paint.transform.localPosition;
                var pos = board.CenterPosAtHex(hex, board.center) + localPos + offset;

                if (!board.IsLegalToPut(paint.Color, pos))
                {
                    legalToPutAtHex = false;
                    break;
                }
            }
            if(legalToPutAtHex) 
            {
                legalToPutSomewhere = true;
                break;
                //count++;
                //Debug.Log("PieceManager: this piece is legal to put at: " + hex);
            }

        }
        //Debug.Log("PieceManager: total placeable slots: " + count);
        return legalToPutSomewhere;

    }
}



