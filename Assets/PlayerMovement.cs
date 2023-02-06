using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Pick_element
    public Image uiPannel;  //to show the current element

    [Header("Reference")]
    SwingController swingController;

    [Header("Movement")]
    public float moveSpeed;

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
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [SerializeField] public bool hasLaunched;
    [SerializeField] private float _launchHoldTimer;
    [SerializeField] private float _launchHoldLimit = 3.0f;


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
        hasLaunched = false;
        _launchHoldTimer = 0.0f;
        _manager = GameObject.Find("Game Manager").GetComponent<Manager>();
        swingController = gameObject.GetComponent<SwingController>();
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

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
        //MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (!hasLaunched)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _launchHoldTimer += Time.deltaTime;
                _launchHoldTimer = Mathf.Clamp(_launchHoldTimer, 0.5f, _launchHoldLimit);
                _manager.UpdateChargeText(_launchHoldTimer);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector3 launchForce = _mainCamera.transform.forward * _launchHoldTimer * 10.0f;
                Debug.Log("Launching: " + launchForce);
                rb.AddForce(launchForce, ForceMode.Impulse);
                hasLaunched = true;
            }
        }

        if(hasLaunched)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Vector3 launchForce = _mainCamera.transform.forward * 5.0f;
                rb.AddForce(launchForce, ForceMode.Impulse);
            }
        }

        // when to jump
        /*if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }*/
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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

    //Function to change the color of UI pannel
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick_Fire"))
        {
            uiPannel.color = new Color(1, 0, 0);
            swingController.element = "Fire";
        }

        if (other.gameObject.CompareTag("Pick_Ice"))
        {
            uiPannel.color = new Color(0, 0, 1);
            swingController.element = "Ice";
        }
    }
}