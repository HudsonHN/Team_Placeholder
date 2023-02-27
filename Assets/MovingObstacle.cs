using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] bool isAround = false;
    [SerializeField] bool isHorizontal = false;
    [SerializeField] bool isVertical = false;
    [SerializeField] float maxMoveRange = 3f; //for move Horizontal, vertical
    [SerializeField] float speed = 1f; //for move Horizontal, vertical
    float radius; //for move Horizontal, vertical


    Vector3 startPos;

    Vector3 pointA, pointB;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        radius = maxMoveRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAround)
        {
            MovingRound();
        }
        else if (isHorizontal)
        {
            MovingHorizontal();
        }
        else if (isVertical)
        {
            MovingVertical();
        }
    }

    void MovingRound()
    {
        float x = startPos.x + radius * Mathf.Sin(speed *Time.time);
        float z = startPos.z + radius * Mathf.Cos(speed * Time.time);
        transform.position = new Vector3(x, transform.position.y, z);
    }

    void MovingHorizontal()
    {
        float x = startPos.x + maxMoveRange * Mathf.Cos(speed * Time.time);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void MovingVertical()
    {
        float y = startPos.y + maxMoveRange * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
