using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Manager.Instance.coinsInLevel <= 0)
        {
            Debug.Log("Level Complete!");
            Manager.Instance.levelCompleteText.gameObject.SetActive(true);
            Manager.Instance.chargeText.gameObject.SetActive(false);
            Manager.Instance.grappleText.gameObject.SetActive(false);
            Manager.Instance.UICanvas.transform.Find("Crosshair").gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not finished yet...");
        }
    }
}
