using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public int coinsInLevel;
    public int grappleLaunchLeft = 7;

    public bool isPaused = false;


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
        UpdateLaunchText(grappleLaunchLeft);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < 0.0f)
        {
            player.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<PlayerMovement>().hasLaunched = false;
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

    public void UpdateChargeText(float chargeAmt)
    {
        chargeText.text = $"Charge: {chargeAmt}";
    }

    public void UpdateLaunchText(int launchAmt)
    {
        grappleText.text = $"Launches Left: {launchAmt}";
    }
}
