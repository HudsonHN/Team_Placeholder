using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public bool canLook = true;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canLook = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Manager.Instance.isPaused && Manager.Instance.canStart && canLook)
        {
            /*if(Input.GetAxis("Mouse ScrollWheel") != 0.0f)
            {
                setCameraX(sensX + Input.GetAxis("Mouse ScrollWheel"));
                setCameraY(sensY + Input.GetAxis("Mouse ScrollWheel"));
            }*/
#if UNITY_EDITOR
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX * 1000.0f;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY * 1000.0f;
#elif UNITY_WEBGL
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX * 65.0f;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY * 65.0f;
#else
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX * 1000.0f;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY * 1000.0f;
#endif
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
            orientation.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
        }
    }

    public void setCamera(float newSens)
    {
        sensX = newSens;
        sensX = Mathf.Clamp(sensX, 0.0f, 2.0f);
        sensY = sensX;
        Manager.Instance.playerSens = sensX;
        Manager.Instance.sensValue.text = $"{sensX.ToString().Truncate(2)}";
    }
}
