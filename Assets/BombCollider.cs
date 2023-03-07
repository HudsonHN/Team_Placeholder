using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BombCollider : MonoBehaviour
{
    private MovingObstacle movingObstacle;
    private float slowspeed;
    GameObject movingObstacleObject;
    int count = 0;
    private bool isSlowing = false;
    void OnTriggerEnter(Collider other)
    {
        movingObstacleObject = GameObject.Find(other.gameObject.name);
        movingObstacle = movingObstacleObject.GetComponent<MovingObstacle>();
        // Debug.Log("SPEED:"+movingObstacle.speed);
        if (movingObstacle != null && isSlowing == false) 
        {
            isSlowing = true;
            if(count==0)
            {
                count++;
                slowspeed = movingObstacle.speed;
            }
        movingObstacle.speed = Mathf.Lerp(slowspeed, 2.0f , 5.0f);
 
        }
    }


    void OnDestroy()
    {
        if (movingObstacle != null  && isSlowing == true)
        {
            isSlowing = false;
            count = 0;
            movingObstacle.speed = Mathf.Lerp(2.0f, slowspeed, 5.0f);
            // Debug.Log("SLOW SPEED:"+slowspeed);
        }
    }


}