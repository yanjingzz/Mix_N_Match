using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PaintManager : MonoBehaviour {

    private SpriteRenderer monsterRenderer, previewRenderer;
    private Paint _color;
    private Paint? preview = null;
    private bool _onBoard;
    private bool fading;
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

            if(_color != value) 
            {
                ChangeSprite(value);
                EventManager.Instance.EncounterColor(value);
                _color = value;
            }


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
        //Debug.Log("Change sprite: " + color);
        fading = true;
        if(OnBoard) HidePreview();
        if (monsterRenderer == null) {
            return false;
        }

        if (color == Paint.Black) {
            Puff(color, 80);
            PlayBlackGrowl();
        }
        if(color == Paint.Empty) {
            if(OnBoard)
                Puff(_color, 80);
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
            FadeIn(monsterRenderer, color, fadeTime);
        }

        return true;
    }

    private bool ShowPreview(Paint paint) 
    {
        if (fading)
        {
            //Debug.Log("Preview but fading");
            return false;
        }
           
        //Debug.Log("Preview: " + paint);
        if (paint == Paint.Empty)
            return false;
        if(previewRenderer.color != paint.ColorValue)
        {
            previewRenderer.DOColor(paint.ColorValue, fadeTime);
            if(paint == Paint.Black)
            {
                PlayBlackGrowl();
            }
        }

        return true;
    }
    private void Puff(Paint paint, int num)
    {
        particleMain.startColor = paint.ColorValue;
        particle.Emit(num);
    }
    private bool HidePreview()
    {
        //Debug.Log("Hiding preview " + preview);
        preview = null;
        previewRenderer.DOFade(0f, fadeTime);
        return true;
    }

    private void FadeIn(SpriteRenderer spriteRenderer, Paint paint, float time) {
        //Debug.Log("Change sprite: " + spriteName);
        spriteRenderer.sprite = paint.MonsterSprite;
        spriteRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        spriteRenderer.DOFade(1f, time).OnComplete(() => {
            fading = false; 
            //HidePreview(); 
        });

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
                preview = Color + otherColor;
                ShowPreview((Paint)preview);
                Puff((Paint)preview, 50);
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.parent.tag == "Board" && collision.gameObject.tag == "Paint"
            && Color != Paint.Empty && Color != Paint.Black)
        {
            var otherColor = collision.gameObject.GetComponent<PaintManager>().Color;

            if (Color + otherColor != preview && Paint.IsMixable(Color, otherColor))
            {
                //Debug.Log("OnTriggerEnter: " + Color);
                ShowPreview(Color + otherColor);
                preview = Color + otherColor;
            }

        }
    }

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
        monsterRenderer.sprite = _color.MonsterSprite;
        monsterRenderer.color = UnityEngine.Color.white;

        HidePreview();
        preview = null;
        fading = false;
    }

    public AudioClip BlackGrowl;

    public void PlayBlackGrowl()
    {
        if (MainMenuManager.Instance.SoundFXOn)
            AudioSource.PlayClipAtPoint(BlackGrowl, Camera.main.transform.position);
    }
    public void Flash()
    {
        Debug.Log("flashin");
        monsterRenderer.color = UnityEngine.Color.black;
        monsterRenderer.DOColor(UnityEngine.Color.white, 0.8f);
    }
}
