using UnityEngine;
using System.Collections;

public class ParticleSizeChanger : MonoBehaviour
{
    public float startScale = 1f;
    public float endScale = 5f;
    public float duration = 5f;

    void Start()
    {
        StartCoroutine(ChangeScale());
    }

    IEnumerator ChangeScale()
    {
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            float currentScale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(endScale, endScale, endScale);
    }
}
