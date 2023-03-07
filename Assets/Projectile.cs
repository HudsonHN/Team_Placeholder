using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float lifeTime = 5.0f;
    public AI_Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * (10.0f * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Do stuff here to penalize player
            boss?.UpdateHP(20);
            Destroy(gameObject);
        }
    }
}
