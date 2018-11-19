using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CustomerView : MonoBehaviour {

    CustomerController _controller;
    Action wentOffScreen;

    public Sprite characterSprite, holdingSprite;
    public float slideTime = 0.8f;
    public float fadeTime = 0.8f;
    public Vector2 StartPosition, EndPosition;
    public SpriteRenderer orderRenderer, characterRenderer;
    public SpriteRenderer bubbleRenderer, holdRenderer, monsterRenderer;
    public ParticleSystem particle;

    public bool Holding
    {
        set
        {
            characterRenderer.sprite = value ? characterSprite : holdingSprite;
        }
    }

    public CustomerController Controller
    {
        get { return _controller; }
        set
        {

            wentOffScreen = value.WentOffScreen;
            _controller = value;
        }
    }


    public void SlideTo(Vector2 position, TweenCallback onComplete = null)
    {
        transform.DOMove(position, slideTime).OnComplete(onComplete);
    }

    public void SlideOff()
    {
        transform.DOMove(EndPosition, slideTime).OnComplete(() =>
        {
            wentOffScreen();
            ResetPosition();
            Hide();
        });
    }

    public void ResetPosition()
    {
        transform.position = StartPosition;
    }


    public void UpdateOrderColor(Paint order)
    {
        bubbleRenderer.enabled = true;
        orderRenderer.enabled = true;
        holdRenderer.enabled = false;
        orderRenderer.sprite = Resources.Load<Sprite>(order.SpriteName);
        characterRenderer.sprite = characterSprite;
        monsterRenderer.enabled = false;
    }

    public void Hide()
    {
        bubbleRenderer.enabled = false;
        orderRenderer.enabled = false;
        holdRenderer.enabled = false;
        characterRenderer.enabled = false;
        monsterRenderer.enabled = false;
    }


    public void Show()
    {
        bubbleRenderer.enabled = true;
        orderRenderer.enabled = true;
        holdRenderer.enabled = true;
        characterRenderer.enabled = true;
        monsterRenderer.enabled = false;
    }

    public void PlayFulfillOrderAnimation(Paint paint)
    {
        var main = particle.main;
        main.startColor = paint.ColorValue;
        particle.Emit(50);
        bubbleRenderer.FadeOff(fadeTime);
        orderRenderer.FadeOff(fadeTime);
        monsterRenderer.FadeOnWithSprite(Resources.Load<Sprite>(paint.SpriteName), fadeTime);

    }

    public void HoldGift()
    {

        bubbleRenderer.enabled = false;
        orderRenderer.enabled = false;
        characterRenderer.sprite = holdingSprite;
        holdRenderer.enabled = true;
    }


}

public static class SpriteExtensions
{
    public static void FadeOff(this SpriteRenderer renderer, float fadeTime)
    {
        renderer.DOFade(0, fadeTime).OnComplete(() => 
        {
            renderer.color = Color.white;
            renderer.enabled = false;
        });
    }

    public static void FadeOnWithSprite(this SpriteRenderer renderer, Sprite sprite ,float fadeTime)
    {
        renderer.enabled = true;
        renderer.color = new Color(1,1,1,0);
        renderer.sprite = sprite;
        renderer.DOFade(1, fadeTime);
    }
}

