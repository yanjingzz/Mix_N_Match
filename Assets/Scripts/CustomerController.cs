using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour {
    [SerializeField] Customer customer;
    [SerializeField] CustomerView customerView;
    CustomerPooler pooler;
    private ArtpieceDisplayer _displayer;
    public List<Vector2> spawnPositions;
    public List<Sprite> CharacterSprites;
    public List<Sprite> HoldingSprites;

    public bool Spawnable 
    {
        get
        {
            return !OnScreen;
        }

    }
    public bool OnScreen { get; private set; }

    public bool CanFulfillOrder { get; private set; }

    private void Start()
    {
        customer.Controller = this;
        customerView.Controller = this;
        _displayer = GameObject.FindWithTag("ArtpieceDisplayer").GetComponent<ArtpieceDisplayer>();
        if(_displayer == null)
        {
            Debug.LogError("Customer controller: missing displayer");
        }
    }

    public Artpiece Art
    {
        get { return customer.Art; }
        set
        {
            customer.Art = value;
        }
    }

    public int StandingIndex
    {
        get { return customer.StandingIndex; }
        set
        {
            customer.StandingIndex = value;
            if (value >= 0)
            {
                customerView.SlideTo(spawnPositions[value]);
            }
        }
    }


    public void Create(int spriteIndex)
    {
        customer.SpriteIndex = spriteIndex;
        customerView.ResetPosition();
        customerView.Hide();
        customerView.characterSprite = CharacterSprites[spriteIndex];
        customerView.holdingSprite = HoldingSprites[spriteIndex];
        customerView.Holding = false;
        CanFulfillOrder = true;
    }




    public void WillGoOnScreen()
    {
        Debug.Log(this + " will go on screen");
        OnScreen = true;
        customerView.Show();
        if (customer.IsOrdering)
        {
            if (customer.Order == null)
            {
                Debug.LogError("Customer: Is ordering but has no order!");
            }
            else
            {
                var order = (Paint)(customer.Order);
                customerView.UpdateOrderColor(order);
            }

        }
        else
        {
            customerView.HoldGift();
        }
    }

    internal void SpawnWithArt(Artpiece art, CustomerPooler customerPooler)
    {
        customer.Art = art;
        pooler = customerPooler;
    }

    public void WentOffScreen()
    {
        OnScreen = false;
        CanFulfillOrder = true;
        customer.StandingIndex = -1;
    }

    public void FulfillOrder()
    {
        customerView.PlayFulfillOrderAnimation(customer.FulfilledOrder());
        CanFulfillOrder = false;
        customerView.Invoke("SlideOff", 1f);
    }

    public override string ToString()
    {
        return "Customer " + customer.SpriteIndex;
    }

    public bool CheckOrderFulfilled (Paint paint) 
    {
        if (customer.IsOrdering) {
            if (paint == customer.Order)
            {
                return true;
            }
        }
        return false;
    }

    private void OnMouseUp()
    {
        if(customer.IsGifting)
        {
            _displayer.DisplayArtpiece(customer.Art, GiftOpened);
        }
    }

    public void GiftOpened() 
    {
        customer.Art.Owned = true;
        customer.Art = null;
        customerView.SlideOff();
        pooler.GiftGiven(customer.StandingIndex);
    }
}
