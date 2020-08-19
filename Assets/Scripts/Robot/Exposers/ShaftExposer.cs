using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShaftExposer : MonoBehaviour
{
    [Inject] private MotorShaft shaft;
    
    public MotorShaft Shaft()
    {
        return shaft;
    }
}
