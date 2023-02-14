using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsObj : MonoBehaviour
{
    public int deathCount;
    public long timeToFinish;
    public string sceneName;

    public AnalyticsObj(){
        deathCount = Manager.Instance.deathCount;
        timeToFinish = Goal.timeLine;
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
