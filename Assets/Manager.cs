using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private GameObject UICanvas;
    private Text chargeText;
    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.Find("UI");
        chargeText = UICanvas.transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeText(float chargeAmt)
    {
        chargeText.text = $"Charge: {chargeAmt}";
    }
}
