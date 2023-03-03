using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour
{
    public GameObject newspherePrefab;
    public GameObject spherePrefab;
    public float spawnDistance = 20f;
    public float despawnTime = 5.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Spawn the sphere 10 units away from the mouse pointer
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * spawnDistance;
            GameObject newSphere = Instantiate(newspherePrefab, spawnPos, Quaternion.identity);

            Manager.Instance.spawnedGrapplePoints++;
            UnityEngine.Debug.Log("number of grapple points: " + Manager.Instance.spawnedGrapplePoints);
        
            // Keep updating the sphere's position to follow the mouse pointer
            StartCoroutine(FollowMouse(newSphere));
        }

        if (Input.GetKeyUp(KeyCode.E))
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
    IEnumerator FollowMouse(GameObject sphere)
    {
        while (Input.GetKey(KeyCode.E))
        {
            // Update the sphere's position to follow the mouse pointer
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * spawnDistance;
            sphere.transform.position = newPos;
        
            yield return null;
        }
    
        // Destroy the sphere when the E key is released
        Destroy(sphere);
    }
}