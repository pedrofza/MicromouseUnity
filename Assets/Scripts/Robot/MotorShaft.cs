using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotorShaft : IRotate
{
    private DCMotor motor;
    private float gearRatio;

    public MotorShaft(DCMotor motor, float gearRatio)
    {
        this.motor = motor;
        this.gearRatio = gearRatio;
    }
    
    public Vector3 GetAngularVelocity()
    {
        return motor.GetAttachedBody().LocalAngularVelocity() * gearRatio;
    }
}