using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtpieceManager : Singleton<ArtpieceManager> {
    protected ArtpieceManager (){}
    public List<Artpiece> artpieces;
    Dictionary<int, HashSet<Artpiece>> piecesByTier = new Dictionary<int, HashSet<Artpiece>>();
    List<Artpiece> availablePiece;
    int _tier = 0;

    private void Awake()
    {
        for (int i = 0; i <= maxTier; i++)
        {
            piecesByTier[i] = new HashSet<Artpiece>();
        }
        foreach (Artpiece piece in artpieces)
        {
            if (!piece.Owned)
            {
                piecesByTier[piece.Tier].Add(piece);
            }
        }
        availablePiece = new List<Artpiece>(piecesByTier[0]);
    }


    public int Tier
    {
        get { return _tier; }
        set 
        {
            int newTier = Mathf.Min(value, maxTier);
            if (newTier <= _tier) return;
            Debug.Log("ArtpieceManager: Updating tier to " + newTier);
            for (int i = _tier + 1; i <= newTier; i++)
            {
                foreach(Artpiece piece in piecesByTier[i]) {
                    availablePiece.Add(piece);
                }
            }
            _tier = newTier;
        }
    }
    private readonly int maxTier = 3;

    public Artpiece RandomArtpiece() {
        if (availablePiece.Count == 0) 
        {
            Debug.Log("ArtpieceManager: No available artpice");
            return null;
        }
        var index = Random.Range(0, availablePiece.Count);
        Artpiece piece = availablePiece[index];
        availablePiece.RemoveAt(index);
        return piece;
    }


}
