using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PaintManager : MonoBehaviour {

    public GameObject blackPrefab;

    private BlackSpotManager blackspot;
    private SpriteRenderer spriteRenderer;
    private Piece.Paint _color;
    private Piece.Paint preview;
    private bool _onBoard;
    private void Awake()
    {
        blackspot = Instantiate(blackPrefab, this.transform, false).GetComponent<BlackSpotManager>();
        if(blackspot == null)
        {
            Debug.Log("Paint: Missing blackspotManager");
        }


        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null) 
        {
            Debug.Log("Paint: Missing sprite renderer");
        }
    }
    public Piece.Paint Color
    {
        get {
            return _color;
        }
        set {

            ChangeSprite(value);
            _color = value;
        }
    }

    public bool OnBoard 
    {
        get 
        {
            return _onBoard;
        }
        set
        {
            if (_onBoard == false && value == true) 
            {
                CircleCollider2D coll = this.GetComponent<CircleCollider2D>();
                if (coll != null) {
                    coll.radius = 0.1f;
                }
            }
            _onBoard = value;
        }
    }
    private bool ChangeSprite(Piece.Paint color) 
    {
        if (spriteRenderer == null) {
            return false;
        }

        if (color == Piece.Paint.Black) {
            blackspot.Show();
        } else {
            blackspot.Hide();
        }

        //delay updating color if putting small piece of paint on board
        //for smoother visual effect
        if (_color == Piece.Paint.Empty && OnBoard)
            Invoke("DelayedUpdateColor", 0.5f);
        else
            spriteRenderer.DOColor(color.color, 0.5f);

        //change scale
        if (color.IsSmall() ) {
            transform.DOScale(0.7f, 0.5f); 
        } else {
            transform.DOScale(1f, 0.5f);
        }
        return true;
    }

    private bool PreviewPaint(Piece.Paint paint) 
    {

        if (paint == Piece.Paint.Empty || paint == preview)
            return false;
        preview = paint;
        return true;
    }

    private bool HidePreview()
    {
        preview = Piece.Paint.Empty;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint" 
            && Color != Piece.Paint.Empty && Color != Piece.Paint.Black)
        {
            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;
            //Debug.Log("OnTriggerEnter: " + (Color + otherColor));
            if (Piece.Paint.IsMixable(Color, otherColor)) 
            {

                PreviewPaint(Color + otherColor);
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint" 
            && Color != Piece.Paint.Empty && Color != Piece.Paint.Black)
        {
            if(spriteRenderer == null) {
                return;
            }
            HidePreview();
                
        }
    }
    private void DelayedUpdateColor() {
        spriteRenderer.color = _color.color;
    }

}
