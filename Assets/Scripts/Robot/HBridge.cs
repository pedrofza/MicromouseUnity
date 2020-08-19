using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HBridge : IHBridge
{
    private IVoltageSource voltageSource;

    private float dutyCycle;
    public float DutyCycle { get => dutyCycle; set => Pwm(value); }

    public HBridge()
    {
        DutyCycle = 0.0f;
    }

    public void SetVoltageSource(IVoltageSource source)
    {
        this.voltageSource = source;
    }

    public float DrawCurrent(float current, float deltaTime)
    {
        if (voltageSource == null)
        {
            Debug.Log("Voltage source not set");
            return 0.0f;
        }
        return voltageSource.DrawCurrent(current, deltaTime);
    }

    public float GetVoltage()
    {
        if(voltageSource == null)
        {
            return 0.0f;
        }

        return DutyCycle * voltageSource.GetVoltage();
    }

    private void Pwm(float dutyCycle)
    {
        float dutyCycleClamp = Mathf.Clamp(dutyCycle, -1.0f, 1.0f);
        if (dutyCycleClamp != dutyCycle)
        {
            Debug.Log("dutyCycle has been clamped from " + dutyCycle + " to " + dutyCycleClamp);
        }
        this.dutyCycle = dutyCycleClamp;
    }
}
