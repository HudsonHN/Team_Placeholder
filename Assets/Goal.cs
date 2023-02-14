using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static long timeLine;
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
            Manager.Instance.levelCompleted = true;
            Manager.timerParse.Stop();
            timeLine = Manager.timerParse.ElapsedTicks/10000000;
            UnityEngine.Debug.Log("HEllo stopwatch - "+ Manager.timerParse.Elapsed.ToString("mm\\:ss"));
            UnityEngine.Debug.Log("HEllo stopwatch - "+ timeLine.ToString());
            PostToDatabase();
        }
        else
        {
            Debug.Log("Not finished yet...");
        }
    }
    private void postToDatabase(){
        AnalyticsObj dbObj = new AnalyticsObj();
        RestClient.Post("https://placeholders-ee91c-default-rtdb.firebaseio.com/.json",dbObj);
        }
}
