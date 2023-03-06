using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MathPickup : MonoBehaviour
{
    public int numToTutorial; //I just created to access number from Tutorial

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            string myText = gameObject.transform.Find("3D Text Prefab").GetComponent<TextScript>().EnterTextHere;
            int num;
            Debug.Log("num");

            if (int.TryParse(myText, out num))
            {
                MathManager.instance.doMath(num);
                numToTutorial = num;
            }
            else
            {
                switch (myText)
                {
                    case "+":
                        MathManager.instance.currOperation = MathManager.operation.add;
                        break;
                    case "-":
                        MathManager.instance.currOperation = MathManager.operation.subtract;
                        break;
                    case "*":
                        MathManager.instance.currOperation = MathManager.operation.multiply;
                        break;
                    case "/":
                        MathManager.instance.currOperation = MathManager.operation.divide;
                        break;
                    default:
                        Debug.Log("invalid operator!");
                        break;
                }
            }
        }
    }
}
