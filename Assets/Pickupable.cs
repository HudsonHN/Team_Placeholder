using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{

    [SerializeField] private float spinRate = 250.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Manager.Instance.coinsInLevel--;
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (spinRate * Time.deltaTime), transform.eulerAngles.z);
    }

}
