using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsObj : MonoBehaviour
{
    public int replayNumber;
    public long levelTimeLine;

    public AnalyticsObj(){
        replayNumber = Manager.Instance.deathCount;
        levelTimeLine = Goal.timeLine;
    }
}
