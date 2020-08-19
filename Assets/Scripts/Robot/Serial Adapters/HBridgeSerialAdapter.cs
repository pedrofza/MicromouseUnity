using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HBridgeSerialAdapter : SimpleSerialAdapterForComponent<HBridge>
{
    [SerializeField] private int bitsOfResolution;

    private void Awake()
    {
        if (bitsOfResolution <= 8 || bitsOfResolution > 16 || bitsOfResolution == 16)
        {
            Debug.LogError("BUG: Not implemented");
        }
    }

    public override bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        if (message.Length < 2)
        {
            bytesRead = 0;
            return false;
        }

        int commandCounts = BitConverter.ToInt16(message, 0);
        int maxAbsValue = MyMath.MaximumValueRepresentedByQuantityOfBits(bitsOfResolution);
        commandCounts = Mathf.Clamp(commandCounts, -maxAbsValue, maxAbsValue);
        float scale = (float)commandCounts / maxAbsValue;
        Component.DutyCycle = scale;

        bytesRead = 2;
        return true;
    }
}
