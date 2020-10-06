using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ManagerSerialAdapter : SimpleSerialAdapter
{
    private const byte enableMessage = 0x02;
    private const byte disableMessage = 0x03;

    // private void Awake()
    // {
    //     sob.EnableSend();
    // }

    public override bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        if (message.Length < 1)
        {
            Debug.Log("The message was too short (len<1) [Not bug]");
            bytesRead = 0;
            return false;
        }
        
        byte msg = message[0];
        switch(msg)
        {
            case enableMessage:
                sob.EnableSend();
                break;
            case disableMessage:
                sob.DisableSend();
                break;
            default:
                Debug.Log("Invalid message received " + msg);
                break;
        }
        bytesRead = 1;
        return true;
    }
}
