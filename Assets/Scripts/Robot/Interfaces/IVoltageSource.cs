using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVoltageSource
{
    float DrawCurrent(float current, float deltaTime);
    float GetVoltage();
}
