using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public string element;
    private bool canGrapple;
    private bool canBoost;
    RaycastHit hit;

    // public accessor
    public bool IsSwinging { get { return isSwinging; } }

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius = 2.0f;
    public Transform predictionPoint;

    public bool justLaunched;
    public Vector3 launchVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        canBoost = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable)
            && (hit.collider.tag.Equals("NeutralPoint") || hit.collider.tag.Equals(element)))
        {
            canGrapple = true;
            Manager.Instance.crosshair.color = Color.green;
        }
        else if (Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out hit, maxSwingDistance, whatIsGrappleable) 
            && (hit.collider.tag.Equals("NeutralPoint") || hit.collider.tag.Equals(element))) 
        {
            canGrapple = true;
            Manager.Instance.crosshair.color = Color.green;
        }
        else
        {
            canGrapple = false;
            Manager.Instance.crosshair.color = Color.white;
        }
        if (pm.hasLaunched && !Manager.Instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartSwing();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopSwing();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isSwinging)
            {
                GrappleLaunch();
            }
        }
    }

    void StartSwing()
    {
        if(canGrapple)
        {
            if(justLaunched)
            {
                justLaunched = false;
                //rb.AddForce(-launchVelocity, ForceMode.Impulse);
            }
            isSwinging = true;
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.5f;
            joint.minDistance = distanceFromPoint * 0.25f;

            if(!Manager.Instance.firstSwing)
            {
                joint.spring = 0.0f;
                joint.damper = 7.0f;
                joint.massScale = 500.5f;
                Manager.Instance.firstSwing = true;
            }
            else
            {
                joint.spring = 4.5f;
                joint.damper = 7.0f;
                joint.massScale = 4.5f;
            }

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            BreakTimer timer = hit.collider.gameObject.GetComponent<BreakTimer>();
            if (timer != null) timer.StartTicking(this);
        }
    }

    public void BreakRope()
    {
        StopSwing();
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

    private void GrappleLaunch()
    {
        if(canBoost)
        {
            Vector3 launchForce = /*(hit.point - transform.position).normalized*/ cam.transform.forward * grappleForce;
            rb.AddForce(launchForce, ForceMode.Impulse);
            canBoost = false;
            Manager.Instance.grappleText.text = "Launch COOLDOWN";
            StartCoroutine(BoostTimer());
        }
    }

    private IEnumerator BoostTimer()
    {
        yield return new WaitForSeconds(0.5f);
        canBoost = true;
        Manager.Instance.grappleText.text = "Launch READY";
    }

    private void LateUpdate()
    {
        DrawRope();
    }

}
