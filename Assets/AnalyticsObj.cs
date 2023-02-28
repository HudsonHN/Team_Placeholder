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

    public string grappleNames;
    public string grappleValues;

    public string coinNames;
    public string coinValues;

    public AnalyticsObj(){
        deathCount = Manager.Instance.deathCount;
        timeToFinish = Goal.timeLine;
        checkpointname = Checkpoint.checkpointname;
        checkpointTimer = Checkpoint.checkpointTimer;
        FirstSwingTimeTaken = SwingController.timeTakenForFirstSwing;

        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        grappleNames = Manager.Instance.grapplePointNames;
        grappleValues = Manager.Instance.grapplePointValues;

        coinNames = Manager.Instance.coinNames;
        coinValues = Manager.Instance.coinValues;
        
        Debug.Log("Analytics TIme taken for first swing : "+FirstSwingTimeTaken);
        // Debug.Log("Analytics TIme taken for first swing : "+checkpointTimes);
        // Debug.Log("Analytics TIme taken for first swing : "+Manager.Instance.checkpointTimes);
        Debug.Log("2 TIme taken for first swing : "+Checkpoint.checkpointname);
        Debug.Log("3 TIme taken for first swing : "+Checkpoint.checkpointTimer);
    }
}
