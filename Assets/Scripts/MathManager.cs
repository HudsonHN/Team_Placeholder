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

    public string lhsString;
    public string rhsString;
    public string operatorString;

    public AI_Boss boss;
    
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
            operatorString = "+";
        }
        else
        {
            currOperation = operation.multiply;
            operatorString = "*";
        }
        UIManager.instance.randNumText.text = $"__ {operatorString} __";

        boss = GameObject.Find("Level").transform.Find("BossParent")?
            .Find("TempBoss")?.GetComponent<AI_Boss>();
    }

    public void doMath(int rhs)
    {
        bool newNum = false;
        int result = Int32.MinValue;
        
        switch (currOperation)
        {
            case operation.add:
                result = currNumber + rhs;
                operatorString = "+";
                break;
            case operation.subtract:
                result = currNumber - rhs;
                operatorString = "-";
                break;
            case operation.multiply:
                result = currNumber * rhs;
                operatorString = "*";
                break;
            case operation.divide:
                result = currNumber / rhs;
                operatorString = "/";
                break;
            default:
                //Debug.LogError("invalid math operation!");
                currNumber = rhs;
                lhsString = rhs.ToString();
                newNum = true;
                break;
        }

        if (result != Int32.MinValue)
        {
            //currNumber = result;
            Debug.Log($"Math operation: {currNumber} & {rhs}");
            Debug.Log("got number: " + result);
            UIManager.instance.DisplayResult(result);
            GenerateRandomOperator();

            boss?.UpdateHP(-result);
        }

        if (!newNum)
        {
            currNumber = Int32.MinValue;
            //randNumber = UnityEngine.Random.Range(randomMin, randomMax);
        }

        //currOperation = operation.none;
        //UIManager.instance.randNumText.text = "Current Random Number: " + randNumber;

    }

    void GenerateRandomOperator()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        int index = /*rand % 4*/ rand % 2;
        switch (index)
        {
            case 0:
                currOperation = operation.add;
                operatorString = "+";
                break;
            case 1:
                currOperation = operation.multiply;
                operatorString = "*";
                break;
            /*case 2:
                currOperation = operation.subtract;
                break;
            case 3:
                currOperation = operation.divide;
                break;*/
        }
        UIManager.instance.randNumText.text = $"__ {operatorString} __";
    }
}
