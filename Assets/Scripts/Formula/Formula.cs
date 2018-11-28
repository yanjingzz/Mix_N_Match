using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Formula
{
    public Paint left, right;
    public Paint Result { get { return left + right; }}

}