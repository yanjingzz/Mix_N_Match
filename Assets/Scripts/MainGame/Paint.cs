using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public struct Paint : IEquatable<Paint>
{
    [SerializeField]
    private byte r, y, b;

    public static readonly Paint Black = new Paint(1, 1, 1);
    public static readonly Paint Empty = new Paint(0, 0, 0);

    public static readonly Paint RedSmall = new Paint(1, 0, 0);
    public static readonly Paint RedBig = new Paint(2, 0, 0);
    public static readonly Paint YellowSmall = new Paint(0, 1, 0);
    public static readonly Paint YellowBig = new Paint(0, 2, 0);
    public static readonly Paint BlueSmall = new Paint(0, 0, 1);
    public static readonly Paint BlueBig = new Paint(0, 0, 2);

    public static readonly Paint GreenSmall = new Paint(0, 1, 1);
    public static readonly Paint GreenBig = new Paint(0, 2, 2);
    public static readonly Paint OrangeSmall = new Paint(1, 1, 0);
    public static readonly Paint OrangeBig = new Paint(2, 2, 0);
    public static readonly Paint VioletSmall = new Paint(1, 0, 1);
    public static readonly Paint VioletBig = new Paint(2, 0, 2);

    public static readonly Paint Amber = new Paint(1, 2, 0);
    public static readonly Paint Vermillion = new Paint(2, 1, 0);
    public static readonly Paint Magenta = new Paint(2, 0, 1);
    public static readonly Paint Indigo = new Paint(1, 0, 2);
    public static readonly Paint Turquoise = new Paint(0, 1, 2);
    public static readonly Paint Lime = new Paint(0, 2, 1);

    public static readonly Paint Slate = new Paint(1, 1, 2);
    public static readonly Paint Olive = new Paint(1, 2, 1);
    public static readonly Paint Brown = new Paint(2, 1, 1);

    public static readonly Paint[] AllPaints = { Empty, RedSmall, YellowSmall, BlueSmall, GreenSmall, OrangeSmall, VioletSmall, RedBig, YellowBig, BlueBig, GreenBig, OrangeBig, VioletBig, Amber, Vermillion, Magenta, Indigo, Turquoise, Lime, Slate, Brown, Olive, Black};
    public static readonly Paint[] Spawnable = { RedSmall, YellowSmall, BlueSmall, GreenSmall, OrangeSmall, VioletSmall };
    public static readonly Paint[] Orderable = { RedBig, YellowBig, BlueBig, GreenBig, OrangeBig, VioletBig, Amber, Vermillion, Magenta, Indigo, Turquoise, Lime, Slate, Brown, Olive };
    public static readonly Dictionary<Paint, List<Formula>> Formulas;

    static Paint()
    {

        Dictionary<Paint, List<Formula>> dict = new Dictionary<Paint, List<Formula>>();
        foreach (Paint left in Spawnable)
        {
            foreach (Paint right in AllPaints) 
            {
                if(right != Empty && right != Black && IsMixable(left, right))
                {
                    Paint result = left + right;
                    if(!dict.ContainsKey(result))
                    {
                        dict.Add(result, new List<Formula>());
                    }
                    if (right.IsSmall() && dict[result].Contains(new Formula { left = right, right = left }))
                        continue;
                    else 
                        dict[result].Add(new Formula {left = left, right = right});
                }
            }
        }
        Formulas = dict;
    }

    private string SpriteName
    {
        get
        {
            return this == Empty ? null : "Images/Monsters/" + ToString();
        }
    }

    private string PreviewName
    {
        get
        {
            return this == Empty ? null : "Images/Previews/" + ToString();
        }
    }

    public Sprite MonsterSprite { get { return Resources.Load<Sprite>(SpriteName); } }

    public Sprite PreviewSprite { get { return Resources.Load<Sprite>(PreviewName); } }

    private static readonly string[] name =
    {
            "Empty", "Red_small", "Red_big", "Yellow_small", "Orange_small", "Vermillion_big",
            "Yellow_big", "Amber_big", "Orange_big",  "Blue_small", "Violet_small", "Magenta_big",
            "Green_small", "Black_big", "Brown_big", "Lime_big", "Olive_big", "Black_big",
            "Blue_big", "Indigo_big", "Violet_big", "Turquoise_big", "Slate_big", "Black_big",
            "Green_big", "Black_big", "Black_big",
    };
    private static readonly Color32[] _colorValues =
    {
            Color.clear,
            new Color32(0xd2, 0x23, 0x2a, 0xFF), //red d2232a
            new Color32(0xd2, 0x23, 0x2a, 0xFF), //red d2232a
            new Color32(0xfc, 0xe4, 0x0b, 0xFF), //yellow fce40b
            new Color32(0xf8, 0x98, 0x1b, 0xFF), //orange f8981b
            new Color32(0xd6, 0x44, 0x1d, 0xFF), //verm d6441d
            new Color32(0xfc, 0xe4, 0x0b, 0xFF), //yellow fce40b
            new Color32(0xfe, 0xca, 0x10, 0xff), //amber feca10
            new Color32(0xf8, 0x98, 0x1b, 0xFF), //orange f8981b
            new Color32(0x00, 0x48, 0x9e, 0xFF), //blue 00489e
            new Color32(0x5f, 0x27, 0x7b, 0xFF), //violet 5f277b
            new Color32(0xc8, 0x1d, 0x6a, 0xFF), //Magenta c81d6a
            new Color32(0x12, 0xa0, 0x4a, 0xFF), //green 12a04a
            Color.black,
            new Color32(0x66, 0x29, 0x2b, 0xFF), // brown 66292B
            new Color32(0x80, 0xbd, 0x00, 0xFF), // lime 80bd00
            new Color32(0x5a, 0x61, 0x2e, 0xFF), // olive 5a612e
            Color.black,
            new Color32(0x00, 0x48, 0x9e, 0xFF), //blue 00489e
            new Color32(0x30, 0x21, 0x90, 0xFF), //indigo 302190
            new Color32(0x5f, 0x27, 0x7b, 0xFF), //violet 5f277b
            new Color32(0x2A, 0xb6, 0xbe, 0xFF), // tur 02929a
            new Color32(0x29, 0x57, 0x79, 0xFF), // slate 295779
            Color.black,
            new Color32(0x12, 0xa0, 0x4a, 0xFF), //green 12a04a
            Color.black,
            Color.black,
        };

    public int MinMatchNum
    {
        get
        {
            //if (IsPrimary || IsSecondary) return 3;
            //if (IsTertiary) return 2;
            //return int.MaxValue;
            if (this == Paint.Black) return int.MaxValue;
            return 3;
        }
    }

    public Paint(byte r, byte y, byte b)
    {
        if (r > 2 || y > 2 || b > 2)
        {
            Debug.LogError("Piece.Color: trying to initiate out of index color" + r + ", " + y + ", " + b);
        }
        if (r + y + b >= 5)
        {
            r = b = y = 1;
        }
        this.r = r;
        this.b = b;
        this.y = y;

    }

    public Paint(int r, int y, int b)
    {
        if (r > 2 || y > 2 || b > 2)
        {
            Debug.LogError("Piece.Color: trying to initiate out of index color" + r + ", " + y + ", " + b);
        }
        if (r + y + b >= 5)
        {
            r = b = y = 1;
        }
        this.r = (byte)r;
        this.b = (byte)b;
        this.y = (byte)y;
    }

    public Color ColorValue
    {
        get
        {
            return _colorValues[r + y * 3 + b * 9];
        }
    }

    public bool IsPrimary
    {
        get
        {
            return (r == 0 && y == 0 && b != 0) || (b == 0 && y == 0 && r != 0) || (r == 0 && b == 0 && y != 0);
        }
    }

    public bool IsSecondary
    {
        get
        {
            if (r == 0 && y == 0 && b == 0) return false;
            return (r == 0 && y == b) || (b == 0 && y == r) || (y == 0 && r == b);
        }
    }

    public bool IsTertiary
    {
        get
        {
            return (r == 1 || y == 1 || b == 1) && (r == 2 || y == 2 || b == 2);
        }
    }
    public bool IsMuddy
    {
        get
        {
            return r + y + b == 4 && (r != 0 && y != 0 && b != 0);
        }
    }

    public override string ToString()
    {
        return name[r + y * 3 + b * 9];
    }



    public static bool IsMixable(Paint left, Paint right)
    {
        if (left == Empty || right == Empty) return true;
        if (!left.IsSmall() && !right.IsSmall()) return false;
        if (left == Black || right == Black)
            return false;
        Paint res = left + right;
        return res != left && res != right;

    }

    public bool IsSmall()
    {
        return r <= 1 && y <= 1 && b <= 1 && r + y + b < 3;
    }

    public Paint ToSmall()
    {
        return (IsSmall() || IsTertiary || this == Paint.Black) 
            ? (this) : new Paint(r / 2, y / 2, b / 2);
    }

    public override bool Equals(object obj)
    {
        return obj is Paint && Equals((Paint)obj);
    }

    public bool Equals(Paint other)
    {
        return this.r == other.r && this.y == other.y && this.b == other.b;
    }

    public override int GetHashCode()
    {

#pragma warning disable RECS0025 // Non-readonly field referenced in 'GetHashCode()'
        return 165851236 + r.GetHashCode() * 13 + y.GetHashCode() * 7 + b.GetHashCode();
#pragma warning restore RECS0025 // Non-readonly field referenced in 'GetHashCode()'

    }

    public static Paint operator +(Paint left, Paint right)
    {
        if (left == Paint.Empty) return right;
        if (right == Paint.Empty) return left;
        int r = left.r + right.r, y = left.y + right.y, b = left.b + right.b;
        int m = _max(r, y, b);
        if (m >= 3)
        {
            r = r == m ? 2 : (r == 0 ? 0 : 1);
            y = y == m ? 2 : (y == 0 ? 0 : 1);
            b = b == m ? 2 : (b == 0 ? 0 : 1);
        }


        if (r <= 1 && y <= 1 && b <= 1 && _min(r, y, b) == 0)
        {
            r *= 2;
            y *= 2;
            b *= 2;
        }
        return new Paint(r, y, b);
    }

    private static int _max(int i, int j, int k)
    {
        return Math.Max(Math.Max(i, j), k);
    }

    private static int _min(int i, int j, int k)
    {
        return Math.Min(Math.Min(i, j), k);
    }

    public static bool operator ==(Paint left, Paint right)
    {
        return left.r == right.r && left.y == right.y && left.b == right.b;
    }

    public static bool operator !=(Paint left, Paint right)
    {
        return left.r != right.r || left.y != right.y || left.b != right.b;
    }

}