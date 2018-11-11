using UnityEngine;

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

    private void Start()
    {
        Spawn();
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
            for (int i = 0; i < numberOfPieces; i++) 
            {
                int numOfColors = SecondaryEnabled ? 6 : 3;
                var color = Paint.Spawnable[UnityEngine.Random.Range(0, numOfColors)];
                manager.CreateGO(new Piece{color = color, hexPos = hex});
                log += color + " at " + hex + ", ";
                //random walk;
                hex = hex.neighbour(Hex.RandomDirection);

            }
            //Debug.Log(log);
        }

    }

}
