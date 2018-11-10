using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlackSpotManager : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    ParticleSystem particle;
    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("Blackspot: Missing sprite renderer");
        }
        particle = GetComponentInChildren<ParticleSystem>();
        if (spriteRenderer == null)
        {
            Debug.Log("Blackspot: Missing particle system");
        }
    }
	
    public void Show() 
    {
        spriteRenderer.DOFade(1, 0.2f);
        particle.Emit(10);
    }

    public void Hide()
    {
        spriteRenderer.DOFade(0, 0.2f);
    }

}
