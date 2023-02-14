using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{

    [SerializeField] private float spinRate = 250.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Transform tag: {transform.tag}");
        Debug.Log($"Other tag: {other.tag}");
        if (transform.tag.Equals("BluePickup"))
        {
            Debug.Log("Setting to blue");
            Manager.Instance.player.GetComponent<SwingController>().element = "BluePoint";
            Manager.Instance.elementImage.color = new Color(0.0f, 0.0f, 1.0f, 0.65f);
            Manager.Instance.coinsInLevel--;
        }
        else if (transform.tag.Equals("RedPickup"))
        {
            Debug.Log("Setting to red");
            Manager.Instance.player.GetComponent<SwingController>().element = "RedPoint";
            Manager.Instance.elementImage.color = new Color(1.0f, 0.0f, 0.0f, 0.65f);
            Manager.Instance.coinsInLevel--;
        }
        else
        {
            Debug.Log("Grabbed coin");
            Manager.Instance.coinsInLevel--;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (spinRate * Time.deltaTime), transform.eulerAngles.z);
    }

}
