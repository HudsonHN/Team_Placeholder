using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that when ticked down to 0, destroys parent gameobj
/// </summary>
public class BreakTimer : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField]
    private float timeToBreak;
    [SerializeField]
    private float warningTime;
    private bool isTicking = false;
    private bool showWarning = false;
    private SwingController controller;

    // Utility
    private MeshRenderer renderer;
    private Color c_Opaque = new Color(1, 1, 1, 1);
    private Color c_Transp = new Color(1, 1, 1, .3f);

    /// <summary>
    /// Signal timer to start ticking, e.g. when swinging starts
    /// </summary>
    public void StartTicking(SwingController swingController)
    {
        isTicking = true;
        controller = swingController;
    }

    /// <summary>
    /// Signal timer to stop ticking and reset, e.g. when player lets go
    /// </summary>
    public void StopTicking()
    {
        isTicking = false;
        showWarning = false;
        timer = timeToBreak;
    }

    private void Awake()
    {
        timer = timeToBreak;
    }
    private void Start()
    {
        if (!TryGetComponent<MeshRenderer>(out renderer))
        {
            Debug.LogError(gameObject.name + " has no MeshRenderer Component.");
        }
    }

    private void Update()
    {
        if (!isTicking)
        {
            if (timer < timeToBreak) timer += Time.deltaTime;
            return;
        }

        if (controller != null && !controller.IsSwinging)
        {
            StopTicking();
            return;
        }

        timer -= Time.deltaTime;
        if (!showWarning && timer <= warningTime)
        {
            showWarning = true;
            StartCoroutine(WarningLoop());
        }
        if (timer <= 0f)
        {
            // Destroy parent gameobj and signal swing controller to let go
            controller?.BreakRope();
            Destroy(gameObject);
        }
    }

    IEnumerator WarningLoop()
    {
        while (showWarning)
        {
            renderer.material.color =
               (renderer.material.color.a == 1) ? c_Transp : c_Opaque;
            yield return new WaitForSeconds(.5f);
        }
    }
}
