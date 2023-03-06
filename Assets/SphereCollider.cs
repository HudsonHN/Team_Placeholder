using System.Collections;
using UnityEngine;

public class SphereCollider : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            Time.timeScale = 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            Time.timeScale = 1f;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
