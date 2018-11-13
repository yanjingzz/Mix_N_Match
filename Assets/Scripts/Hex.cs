using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Hex
{
    [SerializeField]
    public int q, r;

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
    public Hex(Vector2Int hex)
    {
        q = hex.x;
        r = hex.y;
    }
    public Hex(Vector3Int cube)
    {
        if (cube.x + cube.y + cube.z != 0)
        {
            Debug.LogError("Hexagon Cube representation invalid: x+y+z == 0, " + cube);
        }
        r = cube.z;
        q = cube.x;
    }
    Vector3Int Cube
    {
        get
        {
            return new Vector3Int(q, -q - r, r);
        }
        set
        {
            if (value.x + value.y + value.z != 0)
            {
                Debug.LogError("Hexagon Cube representation invalid: x+y+z == 0, " + value);
            }
            r = value.z;
            q = value.x;
        }
    }

    public static readonly Vector2Int[] directions =
    {
            new Vector2Int(+1, 0), new Vector2Int(+1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, +1), new Vector2Int(0, +1)
        };

    public static Vector2Int RandomDirection
    {
        get
        {
            return directions[UnityEngine.Random.Range(0, 6)];
        }

    }


    public Hex neighbour(Vector2Int direction)
    {
        return new Hex(q + direction.x, r + direction.y);
    }

    public override string ToString()
    {
        return "(" + q + ", " + r + ")";
    }
    public static Hex zero
    {
        get
        {
            return new Hex(0, 0);
        }
    }

    public static Hex operator +(Hex left, Hex right)
    {
        return new Hex(left.q + right.q, left.r + right.r);
    }

    public static Hex operator -(Hex left, Hex right)
    {
        return new Hex(left.q - right.q, left.r - right.r);
    }

    public static implicit operator Vector2(Hex hex) {
        return new Vector2(hex.q, hex.r);
    }

}

