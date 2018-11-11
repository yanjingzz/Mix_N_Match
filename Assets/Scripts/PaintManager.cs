using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PaintManager : MonoBehaviour {

    private SpriteRenderer monsterRenderer, previewRenderer;
    private Paint _color;
    //private Paint? preview = null;
    private bool _onBoard;
    private readonly float fadeTime = 0.5f;
    private ParticleSystem particle;
    private ParticleSystem.MainModule particleMain;
    private void Awake()
    {

        monsterRenderer = transform.Find("Monster").GetComponent<SpriteRenderer>();
        previewRenderer = transform.Find("Preview").GetComponent<SpriteRenderer>();

        if (monsterRenderer == null || previewRenderer == null) 
        {
            Debug.Log("Paint: Missing sprite renderer");
        }
        particle = GetComponentInChildren<ParticleSystem>();
        if (particle == null)
        {
            Debug.Log("Blackspot: Missing particle system");
        }
        particleMain = particle.main;

    }
    public Paint Color
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
                    coll.radius = 0.05f;
                }
            }
            _onBoard = value;
        }
    }
    private bool ChangeSprite(Paint color) 
    {
        HidePreview();
        if (monsterRenderer == null) {
            return false;
        }

        if (color == Paint.Black) {
            Puff(color, 50);
        }
        if(color == Paint.Empty) {
            if(OnBoard)
                Puff(_color, 50);
            monsterRenderer.DOFade(0f, fadeTime);
            //Debug.Break();
            return true;
        }
        //delay updating color if putting small piece of paint on board
        //for smoother visual effect
        if (_color == Paint.Empty && OnBoard)
            Invoke("DelayedUpdateColor", fadeTime);
        else
        {
            FadeIn(monsterRenderer, color.SpriteName, fadeTime);
        }

        return true;
    }

    private bool PreviewPaint(Paint paint) 
    {

        if (paint == Paint.Empty)
            return false;
        Puff(paint, 20);
        previewRenderer.DOColor(paint.ColorValue, fadeTime);
        return true;
    }
    private void Puff(Paint paint, int num)
    {
        particleMain.startColor = paint.ColorValue;
        particle.Emit(num);
    }
    private bool HidePreview()
    {
        //preview = null;
        previewRenderer.DOFade(0f, fadeTime);
        return true;
    }

    private void FadeIn(SpriteRenderer spriteRenderer, string spriteName, float time) {
        spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
        spriteRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        spriteRenderer.DOFade(1f, time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint" 
            && Color != Paint.Empty && Color != Paint.Black)
        {
            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;

            if (Paint.IsMixable(Color, otherColor)) 
            {
                //Debug.Log("OnTriggerEnter: " + Color);
                PreviewPaint(Color + otherColor);
            }

        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    //if (preview == null) {
    //        if (OnBoard && collision.gameObject.tag == "Paint"
    //        && Color != Paint.Empty && Color != Paint.Black)
    //        {
    //            Debug.Log("OnTriggerStay: " + Color + " " + preview);
    //            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;
    //            //preview = otherColor + Color;
    //        }
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (OnBoard && collision.gameObject.tag == "Paint" 
            && Color != Paint.Empty && Color != Paint.Black)
        {
            //Debug.Log("OnTriggerExit: " + Color);
            if (monsterRenderer == null) {
                return;
            }
            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;
            //if (preview == Color + otherColor)
                HidePreview();
                
        }
    }
    private void DelayedUpdateColor() {
        monsterRenderer.sprite = Resources.Load<Sprite>(_color.SpriteName);
        monsterRenderer.color = UnityEngine.Color.white;
        //preview = null;
    }

}
