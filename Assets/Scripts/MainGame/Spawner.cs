using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public static Spawner Instance { get; private set; }
    public bool SecondaryEnabled;
    float SinglePieceP 
    {
        get {
            return 0.1f + BoardManager.Instance.FullRatio() * 0.15f;
        }
    }
    float ThreePieceP
    {
        get
        {
            var score = ScoreManager.Instance.Score;
            float p1 = (float)(System.Math.Tanh(score / 2500.0 - 3)+ 1) / 6;
            float p2 = (1f - BoardManager.Instance.FullRatio()) * 0.3f;
            return p1 * p2;
        }
    }
    public Vector2 spawnLocation;
    public GameObject piecePrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        Spawn();
        EventManager.Instance.OnPlaced += Spawn;
        EventManager.Instance.OnUpdatedScore += (int score) =>
        {
            if (score >= 600)
            {
                SecondaryEnabled = true;
            }
        };
    }


    public void Spawn() {
        //Debug.Log("Spawner: spawn");

        GameObject anchor = Instantiate(piecePrefab, spawnLocation, Quaternion.identity);
        PieceManager manager = anchor.GetComponent<PieceManager>();
        if (manager == null) {
            Debug.LogError("Spawner: Missing PieceManager");
        } else {
            string log = "Spawned: ";
            var hex = Hex.zero;
            int numberOfPieces = 1;
            float randomVar = UnityEngine.Random.Range(0f, 1f);
            var p1 = SinglePieceP;
            var p3 = ThreePieceP;
            Debug.Log("p1 = " + p1 + ", p3 = " + p3 + ", rolled " + randomVar);
            if(randomVar > p1)
            {
                numberOfPieces = 2;
                if (randomVar - p1 < p3)
                    numberOfPieces = 3;
            }
                

            List<Hex> hexes = new List<Hex>(5);

            for (int i = 0; i < numberOfPieces; i++)
            {   //random walk;
                if(i==0) {
                    hexes.Add(Hex.zero);
                }
                else {
                    var newHex = hexes[i - 1].neighbour(Hex.RandomDirection);
                    while(hexes.Contains(newHex)) 
                    {
                        newHex = hexes[i - 1].neighbour(Hex.RandomDirection);
                    }
                    hexes.Add(newHex);
                }

            }

            for (int i = 0; i < numberOfPieces; i++) 
            {
                int numOfColors = SecondaryEnabled ? 6 : 3;
                var color = Paint.Spawnable[UnityEngine.Random.Range(0, numOfColors)];
                manager.CreateGO(new Piece{color = color, hexPos = hexes[i]});
                log += color + " at " + hex + ", ";
            }

            manager.CenterOnChildren();
            
            //Debug.Log(log);
        }

    }

}
