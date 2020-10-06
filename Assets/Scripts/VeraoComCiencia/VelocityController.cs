using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

public class VelocityController : MonoBehaviour
{
    
    [SerializeField] private UnityEvent completedEvent;

    [SerializeField] private GameObject body;
    [SerializeField] private float targetLinearSpeed;
    [SerializeField] private float targetAngularSpeed;
    [SerializeField] private float timeToMantain;
    [SerializeField] private float linearPercentageDeviation;
    [SerializeField] private float angularPercentageDeviation;

    [SerializeField] private TMP_Text tfLinear;
    [SerializeField] private TMP_Text tfAngular;
    
    private float currentLinearSpeed = 0.0f;
    private float currentAngularSpeed = 0.0f;

    private float timeWithinLimits = 0.0f;

    private float linQty = 0.0f;
    private float angQty = 0.0f;

    private void Awake()
    {
    }

    private void Update()
    {
        tfLinear.text = $"Linear {currentLinearSpeed:F3} m/s";
        tfAngular.text = $"Angular {currentAngularSpeed:F3} rad/s";
        if (linOk)
        {
            tfLinear.color = Color.green;
        }
        else
        {
            tfLinear.color = Color.red;
        }

        if (angOk)
        {
            tfAngular.color = Color.green;
        }        
        else
        {
            tfAngular.color = Color.red;
        }
    }
    
    private void ResetTime()
    {
        timeWithinLimits = 0.0f;
    }

    private bool CheckWinCondition()
    {
        return timeWithinLimits >= timeToMantain;
    }

    private bool linOk = false;
    private bool angOk = false;

    private void FixedUpdate()
    {
        Rigidbody rigidbody = body.GetComponent<Rigidbody>();
        Vector3 bodyLinearVelocity = rigidbody.velocity;
        Vector3 bodyAngularVelocity = rigidbody.LocalAngularVelocity();

        currentLinearSpeed = bodyLinearVelocity.magnitude;
        currentLinearSpeed = body.transform.InverseTransformDirection(bodyLinearVelocity).z;
        currentAngularSpeed = Vector3.Dot(bodyAngularVelocity, Vector3.up);

        linQty = Mathf.Abs(linearPercentageDeviation * targetLinearSpeed);
        angQty = Mathf.Abs(angularPercentageDeviation * targetAngularSpeed);

        linOk = targetLinearSpeed - linQty < currentLinearSpeed &&  currentLinearSpeed < targetLinearSpeed + linQty;
        angOk = targetAngularSpeed - angQty < currentAngularSpeed &&  currentAngularSpeed < targetAngularSpeed + angQty;
        if (linOk && angOk)
        {
            timeWithinLimits += Time.deltaTime;
        }
        else
        {
            ResetTime();
        }

        if (CheckWinCondition())
        {
            completedEvent.Invoke();
            Destroy(this);
        }
    
    }
}
