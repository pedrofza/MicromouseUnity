using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HBridgeDisplay : MonoBehaviour
{
    private HBridge hBridge;

    [Inject]
    public void Construct(HBridge hBridge)
    {
        this.hBridge = hBridge;
    }

    void Update()
    {
        Debug.Log(hBridge.DutyCycle);
    }

}
