using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MathPickup : MonoBehaviour
{
    public int numToTutorial; //I just created to access number from Tutorial

    public int randomMin = 1;
    public int randomMax = 10;

    [SerializeField] private float respawnDelay = 30.0f;

    private GameObject textObj;
    private TextScript textScript;
    private Collider textCollider;
    private bool canPickup;

    private void Start()
    {
        textScript = gameObject.transform.GetComponentInChildren<TextScript>();
        textObj = textScript.gameObject;
        textCollider = GetComponent<Collider>();
        canPickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canPickup && other.transform.parent.CompareTag("Player"))
        {
            string myText = textScript.EnterTextHere;
            int num;
            Debug.Log("num");

            if (int.TryParse(myText, out num))
            {
                if (MathManager.instance.currNumber == Int32.MinValue)
                {
                    MathManager.instance.currNumber = num;
                    MathManager.instance.lhsString = myText;
                    UIManager.instance.randNumText.text = $"{myText} {MathManager.instance.operatorString} __";
                }
                else
                {
                    MathManager.instance.rhsString = myText;
                    UIManager.instance.randNumText.text = $"{MathManager.instance.lhsString} {MathManager.instance.operatorString} {MathManager.instance.rhsString}";
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
            StartCoroutine(RandomizeNumValue(respawnDelay));
        }
    }

    int GenerateRandomValue()
    {
        int value = UnityEngine.Random.Range(randomMin, randomMax);
        while(value == 0)
        {
            value = UnityEngine.Random.Range(randomMin, randomMax);
        }
        return value;
    }

    private IEnumerator RandomizeNumValue(float delay)
    {
        textScript.ShowText(false);
        textCollider.enabled = false;
        canPickup = false;
        textScript.ChangeText(GenerateRandomValue().ToString());
        Debug.Log("Picked up, waiting");
        yield return new WaitForSeconds(delay);
        Debug.Log("Waiting over");
        textScript.ShowText(true);
        textCollider.enabled = true;
        canPickup = true;
    }
}
