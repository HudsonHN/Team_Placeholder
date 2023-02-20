using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    public GameObject UICanvas;
    public Text chargeText;
    public Text grappleText;
    public Text levelCompleteText;
    public GameObject player;
    public GameObject spawnPoint;
    public GameObject pauseCanvas;
    public TextMeshProUGUI sensXText;
    public TextMeshProUGUI sensYText;
    public Text sensText;
    public TextMeshProUGUI instructionText;
    public Image elementImage;
    public Image crosshair;
    public Quaternion initialOrientation;
    public int coinsInLevel;
    public int grappleLaunchLeft = 7;
    public int deathCount;
    public float attemptTimer;
    public bool levelCompleted = false;
    public bool canStart = true;
    public PlayerCamera playerCamera;

    public bool isPaused = false;
    public GameObject goalObject;

    public TextMeshProUGUI coinsLeftText;

    private System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
    public static System.Diagnostics.Stopwatch timerParse;

    Coroutine cameraCoroutine;

    public bool firstSwing = false;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.Find("UI");
        chargeText = UICanvas.transform.Find("Charge Text").GetComponent<Text>();
        grappleText = UICanvas.transform.Find("Grapple Text").GetComponent<Text>();
        levelCompleteText = UICanvas.transform.Find("Level Complete Text").GetComponent<Text>();
        player = GameObject.Find("PlayerCapsule");
        spawnPoint = GameObject.Find("SpawnPoint");
        pauseCanvas = UICanvas.transform.Find("Pause Menu").gameObject;
        sensXText = pauseCanvas.transform.Find("Camera Sensitivity X").Find("Sens Text").GetComponent<TextMeshProUGUI>();
        sensYText = pauseCanvas.transform.Find("Camera Sensitivity Y").Find("Sens Text").GetComponent<TextMeshProUGUI>();
        sensText = UICanvas.transform.Find("Sensitivity Text").GetComponent<Text>();
        instructionText = UICanvas.transform.Find("Instruction Text").GetComponent<TextMeshProUGUI>();
        coinsInLevel = GameObject.Find("Level").transform.Find("Coins").childCount;
        elementImage = UICanvas.transform.Find("Element Image").GetComponent<Image>();
        levelCompleted = false;
        timerParse = Stopwatch.StartNew();
        crosshair = UICanvas.transform.Find("Outline Crosshair").Find("Inner Crosshair").GetComponent<Image>();
        goalObject = GameObject.Find("Level").transform.Find("EndPoint").gameObject;
        playerCamera = GameObject.Find("CameraHolder").transform.Find("MainCamera").GetComponent<PlayerCamera>();
        coinsLeftText = UICanvas.transform.Find("Coins Left Text").GetComponent<TextMeshProUGUI>();
        coinsLeftText.text = $"Gold Coins Left: {coinsInLevel}";
        firstSwing = false;
    }

    void ResetScene()
    {
        attemptTimer = 0.0f;
        deathCount = 0;
        levelCompleted = false;
        crosshair = UICanvas.transform.Find("Outline Crosshair").Find("Inner Crosshair").GetComponent<Image>();
        initialOrientation = player.transform.rotation;
    }

    public IEnumerator StopCamera()
    {
        playerCamera.canLook = false;
        playerCamera.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(0.5f);
        playerCamera.canLook = true;
    }

    public void RespawnPlayer()
    {
        player.transform.SetPositionAndRotation(spawnPoint.transform.position, initialOrientation);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMovement>().hasLaunched = false;
        player.GetComponent<PlayerMovement>().launchHoldTimer = 0.0f;
        if(cameraCoroutine != null)
        {
            StopCoroutine(cameraCoroutine);
        }
        cameraCoroutine = StartCoroutine(StopCamera());
        deathCount++;
        player.GetComponent<SwingController>().launchVelocity = Vector3.zero;
        UpdateChargeText(0.0f);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canStart)
        {
            if (!levelCompleted)
            {
                attemptTimer += Time.deltaTime;
            }
            if (player.transform.position.y < 0.0f)
            {
                RespawnPlayer();
            }
            /*if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!isPaused)
                {
                    Pause();
                }
                else
                {
                    UnPause();
                }
            }*/
        }
    }

    public void Pause()
    {
        //Time.timeScale = 0.0f;
        pauseCanvas.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause()
    {
        //Time.timeScale = 1.0f;
        pauseCanvas.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateCoin()
    {
        coinsInLevel--;
        coinsLeftText.text = $"Gold Coins Left: {coinsInLevel}";
        if(coinsInLevel <= 0)
        {
            goalObject.SetActive(true);
            coinsLeftText.text = "Exit ready!";
        }
    }


    public void UpdateChargeText(float chargeAmt)
    {
        chargeText.text = $"Charge: {chargeAmt}";
    }

    public void UpdateLaunchText(int launchAmt)
    {
        grappleText.text = $"Launches Left: {launchAmt}";
    }
}
