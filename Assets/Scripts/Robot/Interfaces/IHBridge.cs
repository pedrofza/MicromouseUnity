using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHBridge : IVoltageSource
{
    float DutyCycle { set; }
}
