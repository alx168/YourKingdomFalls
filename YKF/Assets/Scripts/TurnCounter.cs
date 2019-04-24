using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    private int divisible = 1;
    public Text mytext = null;
    public int counter = 1;
    // Start is called before the first frame update
    void Start()
    {
        CounterChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CounterChange()
    {
    	divisible++;
        if(divisible % 2 == 0){
        	counter++;
        }
        mytext.text = "Turn:      " + counter;

        
        
    }
}
