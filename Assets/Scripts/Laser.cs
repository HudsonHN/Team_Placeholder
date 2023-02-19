using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject start;
    public GameObject end;

    public bool isOn = true;
    
    private LineRenderer mLine;

    // Start is called before the first frame update
    void Start()
    {
        mLine = GetComponent<LineRenderer>();
        mLine.SetPosition(0, start.transform.position);
        mLine.SetPosition(1, end.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var dir = end.transform.position - start.transform.position;
        dir.Normalize();
        if (Physics.Raycast(start.transform.position, dir, out hit))
        {
            if (hit.collider)
            {
                if (hit.collider.gameObject.transform.parent.CompareTag("Player"))
                {
                    Manager.Instance.RespawnPlayer();
                }
            }
        }
    }
}
