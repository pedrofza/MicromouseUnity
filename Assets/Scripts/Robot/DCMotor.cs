using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class DCMotor : IFixedTickable
{
    // Connected components
    private IVoltageSource voltageSource;
    private Rigidbody attachedRigidbody;
    private Vector3 attachedAxis;

    private MotorShaft shaft; //before reduction box
    private MotorShaft reductedShaft; //after reduction box
    
    // Specification
    private DCMotorSpecification specs;

    public DCMotorSpecification Specification { get; private set; }

    // Internal state
    private float current;

    public DCMotor(DCMotorSpecification specs)
    {
        Specification = specs;
        this.shaft = new MotorShaft(this, specs.GearRatio);
        this.reductedShaft = new MotorShaft(this, 1.0f);
        this.current = 0.0f;
    }

    public void SetTarget(Rigidbody rb, Vector3 axis)
    {
        attachedRigidbody = rb;
        attachedAxis = axis.normalized;
    }

    public void SetVoltageSource(IVoltageSource voltageSource)
    {
        this.voltageSource = voltageSource;
    }

    public void FixedTick()
    {
        if (!attachedRigidbody || attachedAxis == null)
        {
            return;
        }

        float dT = Time.deltaTime;

        // 1. update current
        float L = Specification.L;
        float Kt = Specification.Kt;
        float R = Specification.R;

        float angularSpeed = Vector3.Dot(attachedRigidbody.LocalAngularVelocity(), attachedAxis);
        float backEmf = Kt * angularSpeed;
        float voltage = voltageSource.GetVoltage();
        float expectedCurrent = (voltage + (L * current) / dT - backEmf) / (R + L / dT);
        float drawnCurrent = voltageSource.DrawCurrent(expectedCurrent, dT);
        this.current = drawnCurrent;

        // 2. apply torque
        float torque = Kt * current;
        attachedRigidbody.AddRelativeTorque(torque * attachedAxis);
    }

    public MotorShaft GetShaft()
    {
        return this.shaft;
    }

    public MotorShaft GetReductedShaft()
    {
        return this.reductedShaft;
    }

    public Rigidbody GetAttachedBody()
    {
        return this.attachedRigidbody;
    }
}