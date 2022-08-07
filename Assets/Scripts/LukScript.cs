using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class LukScript : MonoBehaviour
{
    private float _timer;
    private bool _isVisible;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        //gameObject.SetActive(true);
    }

    private void Update()
    {
        if(_isVisible && _timer > 0.4f)
        {
            _meshRenderer.enabled = true;
            _isVisible = false;
        }

        _timer += Time.deltaTime;
    }

    public void DontVisible()
    {
        _meshRenderer.enabled = false;
        _timer = 0;
        _isVisible = true;
    }
}
