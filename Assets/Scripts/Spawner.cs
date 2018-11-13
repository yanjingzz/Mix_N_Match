using UnityEngine;
using System.Collections.Generic;
public class Spawner : MonoBehaviour {

    public static Spawner Instance { get; private set; }
    public bool SecondaryEnabled;
    public float singlePieceP 
    {
        get {
            return 0.1f + ScoreManager.Instance.Score * 0.0001f;
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
            int numberOfPieces = UnityEngine.Random.Range(0f, 1f) < singlePieceP ? 1 : 2;
            List<Hex> hexes = new List<Hex>(5);

            for (int i = 0; i < numberOfPieces; i++)
            {   //random walk;
                hexes.Add( (i == 0) ? Hex.zero : hexes[i-1].neighbour(Hex.RandomDirection) );
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
