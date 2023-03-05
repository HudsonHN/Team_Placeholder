using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public float knockbackForce = 3.0f;

    [SerializeField] private Material outlineMat;
    private Renderer _renderer;
    private Material originalMat;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        originalMat = _renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoveredGrapple()
    {
        if (_renderer == null)
        {
            Debug.LogError("NULL RENDERER");
        }
        else
        {
            _renderer.material = outlineMat;
            _renderer.material.SetColor("_BaseColor", originalMat.GetColor("_BaseColor"));
        }
    }

    public void UnhoveredGrapple()
    {
        _renderer.material = originalMat;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockbackForce, ForceMode.Impulse);
    }
}
