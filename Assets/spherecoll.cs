using System.Collections;
using UnityEngine;

public class spherecoll : MonoBehaviour
{
    public float slowFactor = 0.1f; // factor by which to slow down time
    public float slowDuration = 5f; // duration of the time slowdown
    public float fixedDeltaTime = 0.02f; // the fixed timestep used by the physics simulation
    private float originalFixedDeltaTime; // the original fixed timestep used by the physics simulation

private IEnumerator SlowTime()
{
    float currentTimeScale = Time.timeScale;
    float targetTimeScale = 0.5f;
    float lerpTime = 0f;
    while (lerpTime < 1f)
    {
        Time.timeScale = Mathf.Lerp(currentTimeScale, targetTimeScale, lerpTime);
        lerpTime += Time.unscaledDeltaTime / 5f;
        yield return null;
    }
}

private void OnTriggerEnter(Collider other)
{
    
        StartCoroutine(SlowTime());
    
}

private void OnTriggerExit(Collider other)
{
        Time.timeScale = 1f;
    
}
private void OnDestroy()
{
        Time.timeScale = 1f;
    
}
}