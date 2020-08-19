using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battery : IVoltageSource
{
    private float voltage;

    public Battery(float voltage)
    {
        this.voltage = voltage;
    }

    public float DrawCurrent(float current, float deltaTime)
    {
        return current;
    }

    public float GetVoltage()
    {
        return voltage;
    }
}
