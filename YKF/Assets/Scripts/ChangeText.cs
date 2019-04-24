using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public Text mytext = null;
    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TextChange()
    {
        counter++;
        if (counter % 2 == 1) {
            mytext.text = "Computer's Turn";
        }
        else
        {
            mytext.text = "Player's Turn";
        }
        TurnMan.EndTurn();

    }
}
