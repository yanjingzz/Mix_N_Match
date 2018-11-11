using System;
using System.Collections.Generic;
using UnityEngine;


public struct Paint : IEquatable<Paint>
{
    private readonly byte r, y, b;
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

    public static readonly Paint[] Spawnable = { RedSmall, YellowSmall, BlueSmall, GreenSmall, OrangeSmall, VioletSmall };

    public string SpriteName
    {
        get
        {
            //string s = "Images/";
            //if (IsSmall())
            //{
            //    s += "Small/";
            //}
            //else
            //{
            //    s += "Big/";
            //}
            //s += name[r + y * 3 + b * 9];
            //return s;
            if (this == Paint.Black) { return "Images/Black_big"; }
            return "Images/White";

        }
    }



    private static readonly string[] name =
    {
            "Empty", "Red_small", "Red_big", "Yellow_small", "Orange_small", "Vermillion_big",
            "Yellow_big", "Amber_big", "Orange_big",  "Blue_small", "Violet_small", "Magenta_big",
            "Green_small", "Black_big", "Brown_big", "Lime_big", "Olive_big", "Black_big",
            "Blue_big", "Indigo_big", "Violet_big", "Turquoise_big", "Slate_big", "Black_big",
            "Green_big", "Black_big", "Black_big",
        };
    private static readonly Color32[] colors =
    {
            Color.clear,
            new Color32(0xf9, 0x43, 0x4b, 0xFF), //red
            new Color32(0xf9, 0x43, 0x4b, 0xFF), //red f9434b
            new Color32(0xfc, 0xe4, 0x0b, 0xFF), //yellow
            new Color32(0xff, 0x9c, 0x1c, 0xFF), //orange ff9c1c
            new Color32(0xef, 0x56, 0x2d, 0xFF), //verm ef562d
            new Color32(0xfc, 0xe4, 0x0b, 0xFF), //yellow
            new Color32(0xfe, 0xca, 0x10, 0xff), //amber
            new Color32(0xff, 0x9c, 0x1c, 0xFF), //orange ff9c1c
            new Color32(0x0c, 0x57, 0xb0, 0xFF), //blue 0c57b0
            new Color32(0x7f, 0x36, 0xa4, 0xFF), //violet 7F36A4
            new Color32(0xc8, 0x1d, 0x6a, 0xFF), //Magenta c81d6a
            new Color32(0x42, 0xb9, 0x71, 0xFF), //green 42b971
            Color.black,
            new Color32(0x66, 0x29, 0x2b, 0xFF), // brown 66292B
            new Color32(0xAF, 0xF3, 0x38, 0xFF), // lime AFF338
            new Color32(0x5a, 0x61, 0x2e, 0xFF), // olive 5a612e
            Color.black,
            new Color32(0x0c, 0x57, 0xb0, 0xFF), //blue 00489e
            new Color32(0x4A, 0x36, 0xc6, 0xFF), //indigo 4A36C6
            new Color32(0x7f, 0x36, 0xa4, 0xFF), //violet 7F36A4
            new Color32(0x2A, 0xb6, 0xbe, 0xFF), // tur 2AB6BE
            new Color32(0x29, 0x57, 0x79, 0xFF), // slate 295779
            Color.black,
            new Color32(0x42, 0xb9, 0x71, 0xFF), //green 42b971
            Color.black,
            Color.black,
        };

    public int minCount
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

    public Color color
    {
        get
        {
            return colors[r + y * 3 + b * 9];
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
        if (IsSmall() || IsTertiary || this == Paint.Black) return this;
        return (new Paint(r / 2, y / 2, b / 2));
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

        return 165851236 + r.GetHashCode() * 13 + y.GetHashCode() * 7 + b.GetHashCode();

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