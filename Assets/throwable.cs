using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour
{
    public GameObject spherePrefab;
    public float spawnDistance = 5f;
    public float despawnTime = 5.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get the camera's position and forward direction
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;

            // Calculate the spawn position based on the camera's position and forward direction
            Vector3 spawnPos = cameraPos + cameraForward * spawnDistance;

            // Spawn the sphere at the calculated position
            GameObject newSphere= Instantiate(spherePrefab, spawnPos, Quaternion.identity);
            Destroy(newSphere, despawnTime);
        }
    }
}