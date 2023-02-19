using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;
    public bool Activated { get { return activated; } }
    public GameObject spawnPoint;
    private MeshRenderer renderer;

    private Color c_activated = new Color(0, 183f / 255f, 77f / 255f);

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        ActivateCheckpoint();
        /*if(collision.collider.gameObject.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }*/
    }
}
