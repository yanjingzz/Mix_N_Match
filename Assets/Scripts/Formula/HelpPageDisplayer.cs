using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPageDisplayer : MonoBehaviour
{
    public GameObject content;
    public GameObject FormulaPrefab;
    public Text pageNumber;
    public Button nextButton, prevButton;
    public Text TitleText;
    public Text HelpText;
    private int _page;

    public int Page {
        get { return _page; }
        set 
        {
            if(value <= 14 && value >= 0)
            {
                _page = value;
                ClearContent();
                DisplayPage();
                pageNumber.text = "Page " + (_page + 1) + " / 15";
                nextButton.interactable = value != 14;
                prevButton.interactable = value != 0;
            }
                
        }
    }



    // Use this for initialization
    void Start()
    {
        DisplayPage();
    }

    //private void OnValidate()
    //{
    //    //PopulateFormulas();
    //}

    private void PopulateFormulas(Paint paint)
    {
        if(paint != Paint.Empty)
        {
            if(Paint.Formulas.ContainsKey(paint))
            {
                var listOfFormulas = Paint.Formulas[paint];
                foreach (Formula formula in listOfFormulas)
                {
                    var formulaGO = Instantiate(FormulaPrefab, content.transform);
                    var displayer = formulaGO.GetComponent<FormulaDisplayer>();
                    displayer.Formula = formula;
                }
            }
        }

    }
    public void nextPage()
    {
        Page += 1;



 
    }

    public void previousPage()
    {
        Page -= 1;
    }

    private void DisplayPage()
    {
        if(Page == 0)
        {
            TitleText.text = "How to play";
            HelpText.text = "- Drag eggs onto the nests. Putting eggs on monsters makes them change color. Discover the formulas yourself!\n\n" +
            "- Match monsters by putting 3 + of the same monsters next to each other.\n\n" +
            "- Matching different colored monsters can give you powerups. Fulfill orders of customers for more surprises!\n\n" + 
            "- Beware of the black monsters! They can't mix or match with others!";
        } 
        else if (Page == 1)
        {
            TitleText.text = "Primary colors";
            HelpText.text = "";
            PopulateFormulas(Paint.RedBig);
            PopulateFormulas(Paint.YellowBig);
            PopulateFormulas(Paint.BlueBig);
        }
        else 
        {
            Paint paint = Paint.AllPaints[Page + 8];
            if (GameManager.HasEncountered(paint))
                TitleText.text = paint.ToString().Split('_')[0];
            else
                TitleText.text = "???";
            HelpText.text = "";
            PopulateFormulas(paint);
        }
    }
    private void ClearContent() 
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
