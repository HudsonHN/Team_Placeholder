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
    public int randNumber = Int32.MinValue;
    //public int gainedNumber = 0;

    [HideInInspector]
    // result of operation
    public int currNumber = Int32.MinValue;
    
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
        int rand = UnityEngine.Random.Range(0, 10);
        //UIManager.instance.randNumText.text = "Current Random Number: " + randNumber;
        if (rand % 2 == 0)
        {
            currOperation = operation.add;
        }
        else
        {
            currOperation = operation.multiply;
        }
        UIManager.instance.randNumText.text = "Current Operation: " + currOperation;
    }

    public void doMath(int rhs)
    {
        bool newNum = false;
        int result = Int32.MinValue;
        
        switch (currOperation)
        {
            case operation.add:
                result = currNumber + rhs;
                break;
            case operation.subtract:
                result = currNumber - rhs;
                break;
            case operation.multiply:
                result = currNumber * rhs;
                break;
            case operation.divide:
                result = currNumber / rhs;
                break;
            default:
                //Debug.LogError("invalid math operation!");
                currNumber = rhs;
                newNum = true;
                break;
        }

        if (result != Int32.MinValue)
        {
            //currNumber = result;
            Debug.Log("got number: " + result);
            UIManager.instance.DisplayResult(result);
            GenerateRamdomOperator();
        }

        if (!newNum)
        {
            currNumber = Int32.MinValue;
            //randNumber = UnityEngine.Random.Range(randomMin, randomMax);
        }

        //currOperation = operation.none;
        //UIManager.instance.randNumText.text = "Current Random Number: " + randNumber;

    }

    void GenerateRamdomOperator()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        int index = rand % 4;
        switch (index)
        {
            case 0:
                currOperation = operation.add;
                break;
            case 1:
                currOperation = operation.multiply;
                break;
            case 2:
                currOperation = operation.subtract;
                break;
            case 3:
                currOperation = operation.divide;
                break;
        }
        UIManager.instance.randNumText.text = "Current Operator: " + currOperation;
    }
}
