using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct Piece
{
    public Paint color;
    public Hex hexPos;
    public override string ToString()
    {
        return color.ToString() + " at pos " + hexPos;
    }
}