using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;
    public bool Activated { get { return activated; } }
    public GameObject spawnPoint;
    private MeshRenderer renderer;
    private float startTime;
    public static string checkpointname;
    public static string checkname = "initial";
    public static string checkpointTimer;
    private Color c_activated = new Color(0, 183f / 255f, 77f / 255f);
    public static float t = 0.0f;

    void Start()
    { 
        startTime = Time.time;
    }

    public void ActivateCheckpoint()
    {
        if (activated) return;

        activated = true;
        if (spawnPoint != null)
        {
            spawnPoint.transform.position = transform.position + new Vector3(0, 2f, 0);
        }

        if (TryGetComponent<MeshRenderer>(out renderer))
        {
            renderer.material.color = c_activated;
        }

        // Get the name of the checkpoint
        string tempcheckpointName = gameObject.name;
        checkname = tempcheckpointName;
        checkpointname = checkpointname + "," + tempcheckpointName;
        // Calculate the time taken to reach the checkpoint
        float timeTaken = Time.time - startTime;
        t = timeTaken;
        Debug.Log("Timetaken:"+ timeTaken+", Time.time"+Time.time+", starttime"+startTime);
        checkpointTimer = checkpointTimer + "," + timeTaken.ToString();
        Debug.Log("TIME SCALE:"+ Time.timeScale);

        Debug.Log("Time taken to reach checkpoint " + tempcheckpointName + ": " + timeTaken + " seconds");
        Manager.Instance.checkpointTimes[tempcheckpointName] = timeTaken;
    }
    private void OnCollisionEnter(Collision collision)
    {

            ActivateCheckpoint();
    }
}
