using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; //please keep this as public, it is using on 

    public bool movePlayer; // prevent user from using keyboard during tutorials

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [HideInInspector] public bool grounded;

    public Transform orientation;

    //Made it public to prevent character from moving out after unfreeze input
    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public float launchHoldTimer;
    public float launchHoldLimit = 2.0f;

    [SerializeField] private Collider _collider;
    private GameObject _mainCamera;
    private Manager _manager;

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        launchHoldTimer = 0.0f;
        _manager = GameObject.Find("Game Manager").GetComponent<Manager>();
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if(!grounded)
        {
            _collider.material.dynamicFriction = 10.0f;
        }
        else
        {
            _collider.material.dynamicFriction = 0.1f;
        }

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if(!Manager.Instance.isPaused && Manager.Instance.canStart && movePlayer)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            /*if (!hasLaunched)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    launchHoldTimer += Time.deltaTime;
                    launchHoldTimer = Mathf.Clamp(launchHoldTimer, 0.0f, launchHoldLimit);
                    _manager.UpdateChargeText(launchHoldTimer);
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Vector3 launchForce = _mainCamera.transform.forward * launchHoldTimer * 10.0f;
                    Debug.Log("Launching: " + launchForce);
                    rb.AddForce(launchForce, ForceMode.Impulse);
                    SwingController swingController = GetComponent<SwingController>();
                    swingController.justLaunched = true;
                    swingController.launchVelocity = launchForce;
                    hasLaunched = true;
                    Manager.Instance.instructionText.text = "Left Click: Swing\r\nRight Click: Boost";
                }
            }*/

            /*if(hasLaunched)
            {
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    Vector3 launchForce = _mainCamera.transform.forward * 5.0f;
                    rb.AddForce(launchForce, ForceMode.Impulse);
                }
            }*/

            // when to jump
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 2.15f, ForceMode.Force);

        // in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 1.5f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}