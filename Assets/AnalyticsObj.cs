using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsObj : MonoBehaviour
{
    public int deathCount;
    public long timeToFinish;
    public string sceneName;
    public long FirstSwingTimeTaken;

    public string checkpointname;
    public string checkpointTimer;

    public AnalyticsObj(){
        deathCount = Manager.Instance.deathCount;
        timeToFinish = Goal.timeLine;
        checkpointname = Checkpoint.checkpointname;
        checkpointTimer = Checkpoint.checkpointTimer;
        FirstSwingTimeTaken = SwingController.timeTakenForFirstSwing;

        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        Debug.Log("Analytics TIme taken for first swing : "+FirstSwingTimeTaken);
        // Debug.Log("Analytics TIme taken for first swing : "+checkpointTimes);
        // Debug.Log("Analytics TIme taken for first swing : "+Manager.Instance.checkpointTimes);
        Debug.Log("2 TIme taken for first swing : "+Checkpoint.checkpointname);
        Debug.Log("3 TIme taken for first swing : "+Checkpoint.checkpointTimer);
    }
}
