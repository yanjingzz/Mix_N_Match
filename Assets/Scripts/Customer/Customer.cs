using System;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    CustomerController _controller;
    private bool _isOrdering = true;
    private int _orderIndex = -1;
    private int _standingIndex = -1;
    private Artpiece _artpiece;
    private Action willGoOnScreen;

    public bool IsOrdering {get { return _isOrdering; }}
    public bool IsGifting { get { return !_isOrdering; }}

    public CustomerController Controller
    {
        get { return _controller; }
        set
        {

            willGoOnScreen = value.WillGoOnScreen;
            _controller = value;
        }
    }


    public Artpiece Art 
    {
        get { return _artpiece; }
        set { 
            _artpiece = value; 
            _isOrdering = true;
            _orderIndex = 0;
        }
    }

    public Paint? Order
    {
        get {
            if(_artpiece != null)
            {
                if(_orderIndex >= 0 && _orderIndex < _artpiece.Colors.Count)
                {
                    return _artpiece.Colors[_orderIndex];
                }
            }
            return null;
        }
    }

    public int StandingIndex 
    {
        get { return _standingIndex; }
        set
        {
            if(_standingIndex == value) 
            {
                return;
            }
            if(_standingIndex < 0 && value >= 0) {
                if (willGoOnScreen != null)
                    willGoOnScreen();
            }
            _standingIndex = value;

        }
    }
    public int SpriteIndex { get; set; }

    public Paint FulfilledOrder()
    {
        Paint color = (Paint)Order;
        if (_artpiece == null)
        {
            Debug.LogError("Customer manager: " + this + ": missing artpiece to finish");
        }
        _orderIndex++;
        if (_orderIndex >= _artpiece.Colors.Count)
        {
            _isOrdering = false;
        }
        return color;
    }




}
