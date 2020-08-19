using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnlimitAngularVelocity : MonoBehaviour
{
    private float maxCache;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxCache = rb.maxAngularVelocity;
    }

    private void OnEnable()
    {
        rb.maxAngularVelocity = Mathf.Infinity;
    }

    private void OnDisable()
    {
        rb.maxAngularVelocity = maxCache;
    }
}
