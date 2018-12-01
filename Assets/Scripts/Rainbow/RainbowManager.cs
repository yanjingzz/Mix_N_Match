using System.Collections;
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
    public bool HasFull
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

        EventManager.Instance.OnMatched += Matched;
            
    }

    private void Start()
    {
        if (IsFullAtStart) 
        {
            HasFull = true;
            fullRainbow.SetActive(true);
        }
        rainbowRenderer = fullRainbow.GetComponent<SpriteRenderer>();
        if(rainbowRenderer == null) {
            Debug.LogWarning("RainbowManager: Rainbow renderer not found");
        }
    }




    public void Matched (Paint paint, int matches) {

        if (hasRed == false && 
            (paint == Paint.RedBig || paint == Paint.Vermillion || paint == Paint.Magenta) )
        {
            hasRed = true;
            redPiece.SetActive(true);
        }
        else if (hasOrange == false && 
                 (paint == Paint.OrangeBig || paint == Paint.Amber || paint == Paint.Vermillion))
        {
            hasOrange = true;
            orangePiece.SetActive(true);
        }
        else if (hasYellow == false && 
                 (paint == Paint.YellowBig || paint == Paint.Lime || paint == Paint.Amber) )
        {
            hasYellow = true;
            yellowPiece.SetActive(true);
        }
        else if (hasGreen == false &&
                 (paint == Paint.GreenBig || paint == Paint.Turquoise || paint == Paint.Lime))
        {
            hasGreen = true;
            greenPiece.SetActive(true);
        }
        else if (hasBlue == false &&
                 (paint == Paint.BlueBig || paint == Paint.Indigo || paint == Paint.Turquoise))
        {
            hasBlue = true;
            bluePiece.SetActive(true);
        }
        else if (hasViolet == false &&
                 (paint == Paint.VioletBig || paint == Paint.Magenta || paint == Paint.Indigo))
        {
            hasViolet = true;
            violetPiece.SetActive(true);
        }
        if (HasFull) {
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
        if (!board.IsOnBoard(tf.position) || board[hex] == Paint.Empty)
        {
            tf.DOLocalMove(Vector3.zero, 0.2f);
            return;
        }

        board[hex] = Paint.Empty;
        foreach (var dir in Hex.directions)
        {
            board[hex.neighbour(dir)] = Paint.Empty;
        }
        var particles = Instantiate(RainbowParticles, tf, false);
        Destroy(particles, 3f);

        rainbowRenderer.DOFade(0f, 1f).OnComplete(() => 
        { 
            fullRainbow.SetActive(false); 
            tf.localPosition = Vector3.zero;
            rainbowRenderer.color = Color.white;
        });
        EventManager.Instance.Bomb();
        HasFull = false;
    }
}
