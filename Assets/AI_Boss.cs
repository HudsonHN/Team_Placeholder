using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float rotationRate = 30.0f;
    [SerializeField] float castDistance = 100.0f;
    [SerializeField] float shootingMoveDelay = 2.0f;
    [SerializeField] LayerMask whatIsTargeted;
    [SerializeField] Color laserColor = Color.cyan;
    [SerializeField] Color detectedLaserColor = Color.red;
    [SerializeField] float knockbackForce = 10.0f;
    private Transform playerTransform;
    private float fireTimer;
    private LineRenderer lr;
    private RaycastHit hit;
    private bool shouldMove = true;
    private Coroutine stopMoving;
    private Transform startPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("PlayerCapsule").transform;
        startPoint = transform.Find("Capsule");
        lr = GetComponent<LineRenderer>();
        lr.material.color = laserColor;
    }

    // Update is called once per frame
    private void Update()
    {
        if(shouldMove)
        {
            Quaternion lookRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationRate * Time.deltaTime);
            if(fireTimer >= fireRate && Physics.Raycast(startPoint.position, startPoint.forward, out hit, castDistance, whatIsTargeted))
            {
                fireTimer = 0.0f;
                ShootProjectile();
                StartCoroutine(StopMoving(shootingMoveDelay));
            }
            else if(fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }
        }   
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockbackForce, ForceMode.Impulse);
    }

    private IEnumerator StopMoving(float delay)
    {
        shouldMove = false;
        lr.material.color = detectedLaserColor;
        yield return new WaitForSeconds(delay);
        shouldMove = true;
        lr.material.color = laserColor;
    }

    private void LateUpdate()
    {
        lr.SetPosition(0, startPoint.position);
        lr.SetPosition(1, startPoint.position + (startPoint.forward * castDistance));
    }

    private void ShootProjectile()
    {
        if(projectilePrefab != null) 
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = startPoint.position;
            projectile.transform.rotation = transform.rotation;
        }
    }
}
