using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BombCollider : MonoBehaviour
{
    private MovingObstacle movingObstacle;
    private float slowspeed;
    GameObject movingObstacleObject;
    int count = 0;
    void OnTriggerEnter(Collider other)
    {
        movingObstacleObject = GameObject.Find(other.gameObject.name);
        movingObstacle = movingObstacleObject.GetComponent<MovingObstacle>();
        // Debug.Log("SPEED:"+movingObstacle.speed);
        if (movingObstacle != null)
        {
            if(count==0)
            {
                count++;
                slowspeed = movingObstacle.speed;
            }
            movingObstacle.speed = 2.0f;
        }
    }

    void OnDestroy()
    {
        if (movingObstacle != null)
        {
            count = 0;
            movingObstacle.speed = slowspeed;
            Debug.Log("SLOW SPEED:"+slowspeed);
        }
    }
}
