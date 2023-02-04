using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject UICanvas;
    public Text chargeText;
    public Text levelCompleteText;
    public GameObject player;
    public GameObject spawnPoint;

    public int CoinsInLevel;
    public static Manager Instance { get; private set; }
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
        levelCompleteText = UICanvas.transform.Find("Level Complete Text").GetComponent<Text>();
        player = GameObject.Find("PlayerCapsule");
        spawnPoint = GameObject.Find("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < 0.0f)
        {
            player.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);
            player.GetComponent<PlayerMovement>().hasLaunched = false;
        }
    }

    public void UpdateChargeText(float chargeAmt)
    {
        chargeText.text = $"Charge: {chargeAmt}";
    }
}
