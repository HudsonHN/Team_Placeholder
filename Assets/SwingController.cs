using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Stopwatch = System.Diagnostics.Stopwatch;

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
    public float swingRopeDistance = 15.0f;

    [SerializeField] private float grappleForce = 15.0f;

    private PlayerMovement pm;
    private bool isSwinging;
    Vector3 currentGrapplePosition;
    public string element;
    private bool canGrapple;
    private bool canBoost;
    [HideInInspector]
    public bool isPulling = false;
    RaycastHit hit;

    // public accessor
    public bool IsSwinging { get { return isSwinging; } }

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius = 2.0f;
    public Transform predictionPoint;

    public bool justLaunched;
    public Vector3 launchVelocity;
    public bool isMovingGrapple;
    public Transform movingGrappleTransform;
    private Transform pullPointTransform;
    [SerializeField] private Color lrSwingColor = Color.gray;
    [SerializeField] private Color lrPullColor = Color.yellow;

    private Stopwatch firstSwingStopwatch;
    public static long timeTakenForFirstSwing;
    private bool firstSwingComplete = false;
    private GrapplePoint hoveredGrapple;
    public GrapplePoint selectedGrapple;

    public static long giveanyname;

    public float boostTimer = 0.75f;

    public static int numSwings = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        if(!cam)
        {
            Debug.Log("Camera not manually assigned, auto-assigning...");
            cam = GameObject.Find("CameraHolder").transform.Find("MainCamera");
        }
        canBoost = true;
        firstSwingStopwatch = Stopwatch.StartNew();
        lr.material.color = lrSwingColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable)
            && (hit.collider.tag.Equals("NeutralPoint") || hit.collider.tag.Equals(element)))
        {
            canGrapple = true;
            Manager.Instance.crosshair.color = Color.green;
            Manager.Instance.leftClickPrompt.SetActive(true);
            if(hoveredGrapple != null) 
            {
                hoveredGrapple.UnhoveredGrapple();
            }
            hoveredGrapple = hit.transform.GetComponent<GrapplePoint>();
            hoveredGrapple.HoveredGrapple();
        }
        else if (Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out hit, maxSwingDistance, whatIsGrappleable) 
            && (hit.collider.tag.Equals("NeutralPoint") || hit.collider.tag.Equals(element))) 
        {
            canGrapple = true;
            Manager.Instance.crosshair.color = Color.green;
            Manager.Instance.leftClickPrompt.SetActive(true);
            if (hoveredGrapple != null)
            {
                hoveredGrapple.UnhoveredGrapple();
            }
            hoveredGrapple = hit.transform.GetComponent<GrapplePoint>();
            hoveredGrapple.HoveredGrapple();
        }
        else
        {
            canGrapple = false;
            Manager.Instance.crosshair.color = Color.white;
            Manager.Instance.leftClickPrompt.SetActive(false);
            if (hoveredGrapple != null)
            {
                hoveredGrapple.UnhoveredGrapple();
                hoveredGrapple = null;
            }
        }
        if (!Manager.Instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {   
                StartSwing();
            }
            if(Input.GetKey(KeyCode.Mouse0) && isSwinging && !isMovingGrapple)
            {
                Vector3 directionToPoint = swingPoint - transform.position;
                float distanceFromPoint = Vector3.Distance(swingPoint, transform.position);
                if(distanceFromPoint > swingRopeDistance)
                {
                    Debug.Log("DistanceFromPoint: " + distanceFromPoint);
                    rb.AddForce(directionToPoint.normalized * 1500.0f * Time.deltaTime);
                    joint.maxDistance = distanceFromPoint * 0.8f;
                    joint.minDistance = distanceFromPoint * 0.25f;
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StopSwing();
                isMovingGrapple = false;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartPull();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StopPull();
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isSwinging && !pm.grounded)
            {
                GrappleLaunch();
            }
            if(isMovingGrapple && joint && movingGrappleTransform) // temporary fix for null reference errors
            {
                joint.connectedAnchor = movingGrappleTransform.position;
            }
        }
        if (isSwinging && !firstSwingComplete)
        {
            firstSwingComplete = true;
            firstSwingStopwatch.Stop();
            Manager.FirstSwingtimerParse.Stop();
            timeTakenForFirstSwing = Manager.FirstSwingtimerParse.ElapsedTicks / 10000000;
            UnityEngine.Debug.Log("HEllo stopwatch - " + Manager.FirstSwingtimerParse.Elapsed.ToString("mm\\:ss"));
            UnityEngine.Debug.Log("HEllo stopwatch - " + timeTakenForFirstSwing.ToString());
            // firstSwingTimeTaken = firstSwingStopwatch.ElapsedMilliseconds;
            Debug.Log("Time taken for first swing: " + timeTakenForFirstSwing + "ms");
        }
    }

    void StartSwing()
    {
        if(canGrapple && !isPulling)
        {
            if(justLaunched)
            {
                justLaunched = false;
                //rb.AddForce(-launchVelocity, ForceMode.Impulse);
            }

            UnityEngine.Debug.Log("point grappled: " + hit.collider.name);
            if (Manager.Instance.grapplePointsAndCounts.ContainsKey(hit.collider.name))
            {
                Manager.Instance.grapplePointsAndCounts[hit.collider.name] += 1;
                Manager.Instance.hasGrappledAPoint = true;
                Manager.Instance.grapplePointNames = "";
                Manager.Instance.grapplePointValues = "";
            }

            isSwinging = true;
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            selectedGrapple = hit.transform.GetComponent<GrapplePoint>();

            if (hit.transform.GetComponent<MovingObstacle>() != null)
            {
                isMovingGrapple = true;
                movingGrappleTransform = hit.transform;
            }
            else
            {
                isMovingGrapple = false;
            }

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
            if (!firstSwingComplete)
            {
                firstSwingStopwatch.Stop();
                firstSwingComplete = true;
                Manager.FirstSwingtimerParse.Stop();
                timeTakenForFirstSwing = Manager.FirstSwingtimerParse.ElapsedTicks / 10000000;
                Debug.Log("1Time taken for first swing: " + firstSwingStopwatch.ElapsedMilliseconds + "ms");
                Debug.Log("2Time taken for first swing: " + timeTakenForFirstSwing + "ms");
            }
            if (Input.GetMouseButtonDown(0))
            {
                numSwings++;
                Debug.Log("Swing " + numSwings);
            }
        }
    }

    public void BreakRope()
    {
        StopSwing();
        isMovingGrapple = false;
    }

    void StopSwing()
    {
        isSwinging = false;
        lr.positionCount = 0;
        selectedGrapple = null;
        Destroy(joint);
    }

    void StartPull()
    {
        if (canGrapple && !isPulling && !IsSwinging)
        {
            PullPoint pull;
            if (hit.collider.gameObject.TryGetComponent<PullPoint>(out pull))
            {
                isPulling = true;
                pullPointTransform = pull.transform;
                lr.positionCount = 2;
                lrSwingColor = lr.material.color;
                lr.material.color = Color.yellow;
                pull.StartPulling(this);
            }
        }
    }

    void StopPull()
    {
        if (!isPulling) return;
        isPulling = false;
        lr.positionCount = 0;
        lr.material.color = lrSwingColor;
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8.0f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, joint.connectedAnchor);
    }
    void DrawPullLine()
    {
        if (!isPulling) return;
        if (lr.positionCount < 2) return;
        if (!gunTip || !pullPointTransform) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, pullPointTransform.position);
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
        yield return new WaitForSeconds(boostTimer);
        canBoost = true;
        Manager.Instance.grappleText.text = "Launch READY";
    }

    private void LateUpdate()
    {
        DrawRope();
        DrawPullLine();
    }

}
