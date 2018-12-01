using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtpieceManager : Singleton<ArtpieceManager> {
    protected ArtpieceManager (){}
    public List<Artpiece> artpieces;
    Dictionary<int, HashSet<Artpiece>> piecesByTier = new Dictionary<int, HashSet<Artpiece>>();
    List<Artpiece> availablePiece;
    int _tier = 0;
    const string playerPrefsTierKey = "ArtTier";
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

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerPrefsTierKey))
        {
            _tier = PlayerPrefs.GetInt(playerPrefsTierKey);
        }
        else
        {
            _tier = 0;
            PlayerPrefs.SetInt(playerPrefsTierKey, 0);
        }
        MakeArtAvailableToTier(_tier);
        Debug.Log("ArtManager: current tier " + Tier + ", avalable pieces " + availablePiece.Count);
    }


    public int Tier
    {
        get { return _tier; }
        set 
        {
            int newTier = Mathf.Min(value, maxTier);
            if (newTier <= _tier) return;

            MakeArtAvailableToTier(newTier);
            _tier = newTier;
            Debug.Log("ArtpieceManager: Updating tier to " + newTier + ", avalable pieces " + availablePiece.Count);
            PlayerPrefs.SetInt(playerPrefsTierKey,_tier);
            PlayerPrefs.Save();
        }
    }
    private readonly int maxTier = 3;

    public Artpiece RandomArtpiece() {
        if (availablePiece.Count == 0) 
        {
            //Debug.Log("ArtpieceManager: No available artpice");
            return null;
        }
        var index = Random.Range(0, availablePiece.Count);
        Artpiece piece = availablePiece[index];
        availablePiece.RemoveAt(index);
        return piece;
    }

    private void MakeArtAvailableToTier(int tier) 
    {
        for (int i = 0; i <= tier; i++)
        {
            foreach (Artpiece piece in piecesByTier[i])
            {
                if (!piece.Owned && !availablePiece.Contains(piece))
                    availablePiece.Add(piece);
            }
        }
    }


}
