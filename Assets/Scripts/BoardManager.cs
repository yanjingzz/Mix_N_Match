using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{


    protected BoardManager(){}

    public int size = 5;
    public Vector2 center = new Vector2(0, 0);
    public float cellSize = 1;
    public GameObject slotPrefab;
    public GameObject paintPrefab;

    private GameObject boardGO;
    private Dictionary<Hex, Paint> board = new Dictionary<Hex, Paint>();
    private Dictionary<Hex, PaintManager> graphic = new Dictionary<Hex, PaintManager>();

    void Awake()
    {
        //initiating board with black
        var v = new Hex();
        boardGO = new GameObject("Board");
        boardGO.tag = "Board";
        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                if (i + j <= size && -i - j <= size)
                {
                    v.q = i;
                    v.r = j;
                    board[v] = Paint.Empty;
                    Instantiate(slotPrefab, CenterPosAtHex(v, center), Quaternion.Euler(0, 0, 30), boardGO.transform);
                    var paintGO = Instantiate(paintPrefab, CenterPosAtHex(v, center), Quaternion.Euler(0, 0, 0), boardGO.transform);
                    PaintManager manager = paintGO.GetComponent<PaintManager>();
                    if (manager == null)
                    {
                        Debug.Log("Board manager: cannot update board at " + v + ". Missing paint manager");
                    }
                    graphic[v] = manager;
                    manager.OnBoard = true;

                }

            }
        }

    }

    void Start()
    {
        EventManager.Instance.OnPlaced += FindMatches;
    }

    public void FindMatches()
    {
        var queue = new Queue<Hex>();
        var set = new HashSet<Hex>();
        var v = new Hex();
        int totalMatches = 0;
        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                if (i + j <= size && -i - j <= size)
                {
                    v.q = i;
                    v.r = j;
                    var target = board[v];
                    if (!target.IsSmall())
                    {
                        set.Clear();
                        queue.Enqueue(v);
                        while (queue.Count > 0)
                        {
                            var current = queue.Dequeue();
                            foreach (Vector2Int dir in Hex.directions)
                            {
                                var neighbour = current.neighbour(dir);
                                if (board.ContainsKey(neighbour) && board[neighbour] == target)
                                {
                                    if (!set.Contains(neighbour)) {
                                        set.Add(neighbour);
                                        queue.Enqueue(neighbour);
                                    }
                                        
                                }
                            }
                        }
                        //set should contain all cells that are in the same color target;
                        if (set.Count >= target.MinMatchNum)
                        {
                            foreach (Hex hex in set)
                            {
                                this[hex] = Paint.Empty;
                            }
                            Debug.Log("Matched " + set.Count + " pieces of " + target);
                            totalMatches += set.Count;
                            EventManager.Instance.MatchPaint(target, set.Count);
                            
                        }
                    }

                }
            }
        }

    }

    public bool IsLegalToPut(Paint color, Vector2 pos)
    {
        var hex = HexAtPoint(pos, center);
        return IsLegalToPut(color, hex);
    }

    public bool IsOnBoard(Vector2 pos)
    {
        var hex = HexAtPoint(pos, center);
        return IsOnBoard(hex);
    }

    bool IsLegalToPut(Paint color, Hex hex)
    {
        return IsOnBoard(hex) && Paint.IsMixable(board[hex], color);
    }

    bool IsOnBoard(Hex hex)
    {
        return board.ContainsKey(hex);
    }

    public Paint? this[Hex hex]
    {
        get
        {
            return board.ContainsKey(hex) ? (Paint?)board[hex] : null;
        }
        set
        {
            if (board.ContainsKey(hex) && value != null)
            {
                board[hex] = (Paint)value;
                graphic[hex].Color = (Paint)value;
            }
                
        }

    }




    #region Positioning

    public Hex HexAtPoint(Vector2 pos, Vector2 offset)
    {
        float x = pos.x - offset.x;
        float y = pos.y - offset.y;
        //float q = y / cellSize;
        //float r = (- x * Mathf.Sqrt(3) / 2  + y / 2f) / cellSize;
        float r = -(y + x / Mathf.Sqrt(3)) / cellSize;
        float q = 2 * x / Mathf.Sqrt(3) / cellSize;
        return _hex_round(q, r);

    }

    public Vector2 CenterPosAtHex(Hex hex, Vector2 offset)
    {
        var x = Mathf.Sqrt(3) / 2 * hex.q * cellSize + offset.x;
        var y = (-hex.r - hex.q / 2f) * cellSize + offset.y;
        return new Vector2(x, y);
    }






    private Hex _hex_round(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -q - r;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;
        return new Hex(rx, rz);
    }

    #endregion
}
