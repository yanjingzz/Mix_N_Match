using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PaintManager : MonoBehaviour {

    public GameObject blackPrefab;

    private SpriteRenderer monsterRenderer, previewRenderer;
    private Paint _color;
    private Paint? preview;
    private bool _onBoard;
    private readonly float fadeTime = 0.5f;
    private ParticleSystem particle;
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
            particle.Emit(50);
        }
        if(color == Paint.Empty) {

            FadeOut(monsterRenderer, fadeTime);
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

        if (paint == Paint.Empty || paint == preview)
            return false;
        FadeIn(previewRenderer, paint.PreviewName, fadeTime);
        preview = paint;
        return true;
    }

    private bool HidePreview()
    {
        preview = null;
        FadeOut(previewRenderer, 0.5f);
        return true;
    }

    private void FadeIn(SpriteRenderer spriteRenderer, string spriteName, float time) {
        spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
        spriteRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        spriteRenderer.DOFade(1, time);
    }

    private void FadeOut(SpriteRenderer spriteRenderer, float time)
    {
        spriteRenderer.DOFade(0, time).OnComplete(() => spriteRenderer.sprite = null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (preview != null) return;
        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint" 
            && Color != Paint.Empty && Color != Paint.Black)
        {
            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;
            //Debug.Log("OnTriggerEnter: " + (Color + otherColor));
            if (Paint.IsMixable(Color, otherColor)) 
            {

                PreviewPaint(Color + otherColor);
            }


        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (preview == null) {
            if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint"
            && Color != Paint.Empty && Color != Paint.Black)
            {
                var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;
                //Debug.Log("OnTriggerEnter: " + (Color + otherColor));
                if (Paint.IsMixable(Color, otherColor))
                {

                    PreviewPaint(Color + otherColor);
                }


            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint" 
            && Color != Paint.Empty && Color != Paint.Black)
        {
            if(monsterRenderer == null) {
                return;
            }
            HidePreview();
                
        }
    }
    private void DelayedUpdateColor() {
        monsterRenderer.sprite = Resources.Load<Sprite>(_color.SpriteName);
        monsterRenderer.color = UnityEngine.Color.white;
    }

}
