using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    public bool IsTriggered = false;
    public bool IsSlowed = false;
    public GameObject tutorialObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTriggered)
        {
            IsTriggered = true;
            StartCoroutine(SlowdownTrigger(3.0f));
        }
    }

    public IEnumerator SlowdownTrigger(float delay)
    {
        IsSlowed = false;
        tutorialObject.SetActive(true);
        StartCoroutine(SlowDown(0.5f));
        yield return new WaitUntil(() => IsSlowed == true);
        Debug.Log($"Waiting {delay} seconds.");
        yield return new WaitForSecondsRealtime(delay);
        Debug.Log($"Done waiting.");
        tutorialObject.SetActive(false);
        StartCoroutine(SpeedUp(1.0f));
    }

    public IEnumerator SlowDown(float length)
    {
        UnityEngine.Debug.Log("Starting slow down.");
        float timer = 0.0f;
        while (timer < length)
        {
            Time.timeScale = Mathf.Lerp(1.0f, 0.125f, timer / length);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0.125f;
        IsSlowed = true;
        Debug.Log("Ending slow down.");
    }

    public IEnumerator SpeedUp(float length)
    {
        UnityEngine.Debug.Log("Starting speed up.");
        float timer = 0.0f;
        while (timer < length)
        {
            Time.timeScale = Mathf.Lerp(0.125f, 1.0f, timer / length);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;

        Debug.Log("Ending speed up.");
    }
}
