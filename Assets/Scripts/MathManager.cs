using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class MathManager : MonoBehaviour
{
    public static MathManager instance;
    
    public enum operation
    {
        none,
        add,
        subtract,
        divide,
        multiply
    }

    public operation currOperation;
    
    public int randomMin = -5;
    public int randomMax = 10;

    [HideInInspector]
    // randomly generated number
    public int randNumber = 0;
    //public int gainedNumber = 0;
    
    // result of operation
    private int currNumber = 0;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("MathManager instance already exists!");
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        randNumber = UnityEngine.Random.Range(randomMin, randomMax);
        UIManager.instance.randNumText.text = "Current Random Number: " + randNumber;
    }

    public void doMath(int rhs)
    {
        bool newNum = false;
        int result = Int32.MinValue;
        
        switch (currOperation)
        {
            case operation.add:
                result = randNumber + rhs;
                break;
            case operation.subtract:
                result = randNumber - rhs;
                break;
            case operation.multiply:
                result = randNumber * rhs;
                break;
            case operation.divide:
                result = randNumber / rhs;
                break;
            default:
                //Debug.LogError("invalid math operation!");
                randNumber = rhs;
                newNum = true;
                break;
        }

        if (result != Int32.MinValue)
        {
            currNumber = result;
            //Debug.Log("got number: " + currNumber);
            UIManager.instance.DisplayResult(result);
        }

        if (!newNum)
        {
            randNumber = UnityEngine.Random.Range(randomMin, randomMax);
        }

        currOperation = operation.none;
        UIManager.instance.randNumText.text = "Current Random Number: " + randNumber;

    }
}
