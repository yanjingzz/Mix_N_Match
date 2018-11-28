using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Artpiece: ScriptableObject {

    public List<Paint> Colors;
    public string Title;
    public string Description;
    public Sprite Image;
    public int Tier;
    public bool Owned 
    {
        get { return _owned; }
        set
        {
            if (value) _owned = value;
        }
    }
    [SerializeField] private bool _owned;
    public override string ToString()
    {
        return Title;
    }
}
