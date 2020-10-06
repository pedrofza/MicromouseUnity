using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class IncrementalEncoder : IFixedTickable
{
    private IRotate attachedBody;
    private Vector3 attachedAxis;

    private float accumulatedAngle;
    private int accumulatedCounts;

    public float AnglePerPulse { get => 2 * Mathf.PI / Specification.PPR; }
    public EncoderSpecification Specification { get; private set; }

    public IncrementalEncoder(EncoderSpecification specification)
    {
        Specification = specification;
        this.accumulatedAngle = 0.0f;    
        this.accumulatedCounts = 0;
    }

    public void SetAttachedBody(IRotate body, Vector3 axis)
    {
        this.attachedBody = body;
        this.attachedAxis = axis.normalized;
    }

    public void FixedTick()
    {
        if (attachedBody == null || attachedAxis == null)
        {
            return;
        }

        float angularSpeed = Vector3.Dot(attachedBody.GetAngularVelocity(), attachedAxis);
        float deltaAngle = angularSpeed * Time.deltaTime;
        accumulatedAngle += deltaAngle;
        accumulatedCounts += AngleToCounts(accumulatedAngle, out accumulatedAngle);
        
        
        EncoderMeasurement measurement = new EncoderMeasurement(accumulatedCounts);
        OnNewCounts(measurement);
    }

    private int AngleToCounts(float angle, out float remainder)
    {
        float anglePerPulse = AnglePerPulse;
        int counts = 0;
        while (angle >= anglePerPulse)
        {
            angle -= anglePerPulse;
            counts++;
        }
        while (angle <= -anglePerPulse)
        {
            angle += anglePerPulse;
            counts--;
        }
        remainder = angle;
        return counts;
    }

    public delegate void NewCountsEventHandler<T, U>(T sender , U eventArgs);
    public event NewCountsEventHandler<IncrementalEncoder, EncoderMeasurement> NewMeasurement;
    
    public void OnNewCounts(EncoderMeasurement measurement)
    {
        var handler = NewMeasurement;
        handler?.Invoke(this, measurement);
        accumulatedCounts = 0;
    }
}
