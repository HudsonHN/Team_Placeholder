using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsObj : MonoBehaviour
{
    public int unofficial_deathCount;
    public long unofficial_timeToFinish;
    public long unofficial_FirstSwingTimeTaken;
    
    public string unused_SwingCountcoinNames;
    public string unused_swingCount;
    public string unused_coinNames;
    public string unused_coinValues;


    public string sceneName;
    
    public string checkpointname;
    public string checkpointTimer;

    public string grappleNames;
    public string grappleValues;

    public string laserNames;
    public string laserValues;

    public int numSpawnGrapples;

    public string hintString;

    public string AI_Boss_Hits_String_Count;
    public string AI_Boss_Hits_String_Rotation_Speed_Values;


    public AnalyticsObj(){
        unofficial_deathCount = Manager.Instance.deathCount;
        unofficial_timeToFinish = Goal.timeLine;
        unofficial_FirstSwingTimeTaken = SwingController.timeTakenForFirstSwing;

        unused_SwingCountcoinNames = Pickupable.coinName;
        unused_swingCount = Pickupable.swingCount;

        unused_coinNames = Manager.Instance.coinNames;
        unused_coinValues = Manager.Instance.coinValues;

        //# 0
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //# 1
        checkpointname = Checkpoint.checkpointname;
        checkpointTimer = Checkpoint.checkpointTimer;

        //# 2
        grappleNames = Manager.Instance.grapplePointNames;
        grappleValues = Manager.Instance.grapplePointValues;

        //# 3
        laserNames = Manager.Instance.laserNames;
        laserValues = Manager.Instance.laserValues;

        //# 4
        numSpawnGrapples = Manager.Instance.spawnedGrapplePoints;

        //# 5
        hintString = Checkpoint.hintString;

        //# 6
        AI_Boss_Hits_String_Count = Projectile.AI_Boss_Hits_String_Count;
        AI_Boss_Hits_String_Rotation_Speed_Values = Projectile.AI_Boss_Hits_String_Rotation_Speed_Values;


        
        //Debug.Log("Analytics TIme taken for first swing : "+FirstSwingTimeTaken);
        // Debug.Log("Analytics TIme taken for first swing : "+checkpointTimes);
        // Debug.Log("Analytics TIme taken for first swing : "+Manager.Instance.checkpointTimes);
        //Debug.Log("Analytics checkpoint names : "+Checkpoint.checkpointname);
        //Debug.Log("3 TIme taken for first swing : "+Checkpoint.checkpointTimer);

        //Debug.Log("Analytics coin names : "+Pickupable.coinName);
        //Debug.Log("Analytics swing count : "+Pickupable.swingCount);
    }
}
