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
    private Paint order;

    public bool OnScreen { get { return _index >= 0; }}

    private void Start()
    {
        EventManager.Instance.OnMatched += Matched;
    }

    void WillGoOnScreen () 
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        RandomizeWish();

    }

    void RandomizeWish() 
    {

        int i = Random.Range(0, Paint.Orderable.Length);
        order = Paint.Orderable[i];
        wishRenderer.sprite = Resources.Load<Sprite>(order.SpriteName);
    }

    void SlideTo (Vector2 position) 
    {
        transform.DOMove(position, slideTime);
    }


    public void SlideOff()
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

    public void Matched(Paint paint, int num) {
        if(paint == order) 
        {
            CustomerSpawner.Instance.OrderFulfilled(Index);

        }
    }
}
