using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DCMotorExposer : MonoBehaviour
{
    private DCMotor motor;

    public DCMotor Get()
    {
        return motor;
    }

    [Inject]
    public void Constructor(DCMotor motor)
    {
        this.motor = motor;
    }
}
