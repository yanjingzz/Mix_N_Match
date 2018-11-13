using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerManager : MonoBehaviour {
    public float slideTime = 0.8f;
    public Vector2 leftOffscreenPos;
    public Vector2 rightOffscreenPos;
    public SpriteRenderer wishRenderer;
    public SpriteRenderer characterRenderer;
    public List<Vector2> spawnPositions;
    public int Index {
        get { return _index; }
        set 
        {
            //Debug.Log("Customer manager: change index to " + value);
            if (_index == value)
                return;
            if (_index < 0 && value >= 0)
                WillGoOnScreen();
            _index = value;
            if (_index >= 0) 
            {
                SlideTo(spawnPositions[_index]);
            }
            else
            {
                SlideOff();
            }
                

        }
    }

    private int _index = -1;
    public Paint Order 
    {
        get { return _order; }
        set 
        {
            _order = value;
            wishRenderer.sprite = Resources.Load<Sprite>(value.SpriteName);
        }
    }
    private Paint _order;

    public bool OnScreen { get { return _index >= 0; }}



    void WillGoOnScreen () 
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

    }

    void SlideTo (Vector2 position) 
    {
        transform.DOMove(position, slideTime);
    }


    void SlideOff()
    {
        transform.DOMove(leftOffscreenPos, slideTime).OnComplete(ResetPosition);
    }

    public void ResetPosition() {
        gameObject.SetActive(false);
        transform.position = rightOffscreenPos;
    }

    public override string ToString() {
        return characterRenderer.sprite.name;
    }


}
