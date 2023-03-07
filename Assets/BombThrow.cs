using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public GameObject spherePrefab; // the sphere prefab to be thrown
    public float throwForce = 10f; // the force at which the sphere is thrown
    public float destroyTime = 5f; // the time it takes for the sphere to disappear
    public Camera playerCamera; // the player's camera

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject sphere = Instantiate(spherePrefab, playerCamera.transform.position + playerCamera.transform.forward, playerCamera.transform.rotation); // instantiate the sphere at the crosshair
            Rigidbody rb = sphere.GetComponent<Rigidbody>(); // get the sphere's rigidbody component
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse); // throw the sphere forward
            Destroy(sphere, destroyTime); // destroy the sphere after 5 seconds
        }
    }
}