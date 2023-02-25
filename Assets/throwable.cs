using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public GameObject newspherePrefab;
    public GameObject spherePrefab;
    public float spawnDistance = 20f;
    public float despawnTime = 5.0f;
    public int placeablePointLimit;
    public bool placingPoint;

    private void Start()
    {
        //Manager.Instance.placeablePointsLeft.text = $"Placeable Points Left: {placeablePointLimit}";
    }

    void Update()
    {
        if (!placingPoint && placeablePointLimit > 0 && Input.GetKeyDown(KeyCode.E))
        {
            placingPoint = true;
            // Spawn the sphere 10 units away from the mouse pointer
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * spawnDistance;
            GameObject newSphere = Instantiate(newspherePrefab, spawnPos, Quaternion.identity);

            DecreasePlaceableCount();

            // Keep updating the sphere's position to follow the mouse pointer
            StartCoroutine(FollowMouse(newSphere));
        }

        if (placingPoint && Input.GetKeyUp(KeyCode.E))
        {
            placingPoint = false;
            // Get the camera's position and forward direction
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;

            // Calculate the spawn position based on the camera's position and forward direction
            Vector3 spawnPos = cameraPos + cameraForward * spawnDistance;

            // Spawn the sphere at the calculated position
            GameObject newSphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
            StartCoroutine(Despawn(newSphere, despawnTime));
        }
    }

    IEnumerator Despawn(GameObject gameObject, float timer)
    {
        yield return new WaitForSeconds(timer);
        Manager.Instance.player.GetComponent<SwingController>().BreakRope();
        Destroy(gameObject);
    }

    void DecreasePlaceableCount()
    {
        placeablePointLimit--;
        Manager.Instance.placeablePointsLeft.text = $"Placeable Points Left: {placeablePointLimit}";
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