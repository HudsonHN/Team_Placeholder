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
    private Rigidbody rb;

    [SerializeField] private float grappleForce = 15.0f;

    private PlayerMovement pm;
    private bool isSwinging;
    Vector3 currentGrapplePosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void StartSwing()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            Debug.Log("Hit point. Starting to swing.");
            isSwinging = true;
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
        isSwinging = false;
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
        if(pm.hasLaunched && !Manager.Instance.isPaused)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartSwing();
            }
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopSwing();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1) && !isSwinging)
            {
                if(Manager.Instance.grappleLaunchLeft > 0)
                {
                    GrappleLaunch();
                }
            }
        }
    }

    private void GrappleLaunch()
    {
/*        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {*/
            Vector3 launchForce = /*(hit.point - transform.position).normalized*/ cam.transform.forward * grappleForce;
            rb.AddForce(launchForce, ForceMode.Impulse);
            Manager.Instance.grappleLaunchLeft--;
            Manager.Instance.UpdateLaunchText(Manager.Instance.grappleLaunchLeft);
        //}
    }

    private void LateUpdate()
    {
        DrawRope();
    }

}
