using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    private float maxSwingDistance = 25.0f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    private PlayerMovement pm;
    Vector3 currentGrapplePosition;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    void StartSwing()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            Debug.Log("Hit point. Starting to swing.");
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.5f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7.0f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8.0f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.hasLaunched)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartSwing();
            }
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopSwing();
            }
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

}
