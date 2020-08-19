using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VoltageSourceExposer : MonoBehaviour
{
    [Inject] private IVoltageSource vs;
    
    public IVoltageSource voltageSource()
    {
        return vs;
    }
}
