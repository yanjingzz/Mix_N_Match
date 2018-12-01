using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaDisplayer : MonoBehaviour {
    public Image left, right, result;
    public GameObject leftLock, rightLock, resultLock;
    public Formula Formula {
        get { return _formula; }
        set 
        {
            _formula = value;
            ChangeFormula();
        }
    }
    [SerializeField] private Formula _formula;
    private void ChangeFormula() 
    {
        left.sprite = _formula.left.MonsterSprite;
        right.sprite = _formula.right.MonsterSprite;
        result.sprite = _formula.Result.MonsterSprite;

        left.color = GameManager.HasEncountered(_formula.left) ? Color.white : new Color(0.1f, 0.1f, 0.1f, 1);
        leftLock.SetActive(!GameManager.HasEncountered(_formula.left));
        right.color = GameManager.HasEncountered(_formula.right) ? Color.white : new Color(0.1f, 0.1f, 0.1f, 1);
        rightLock.SetActive(!GameManager.HasEncountered(_formula.right));
        result.color = GameManager.HasEncountered(_formula.Result) ? Color.white : new Color(0.1f, 0.1f, 0.1f, 1);
        resultLock.SetActive(!GameManager.HasEncountered(_formula.Result));

    }
}
