using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MathPickup : MonoBehaviour
{
    public int randomMin = -5;
    public int randomMax = 10;

    public float respawnTime = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            string myText = gameObject.transform.Find("3D Text Prefab").GetComponent<TextScript>().EnterTextHere;
            int num;
            if (int.TryParse(myText, out num))
            {
                if (MathManager.instance.currNumber == Int32.MinValue)
                {
                    MathManager.instance.currNumber = num;
                }
                else
                {
                    MathManager.instance.doMath(num);
                    MathManager.instance.currNumber = Int32.MinValue;
                }
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
