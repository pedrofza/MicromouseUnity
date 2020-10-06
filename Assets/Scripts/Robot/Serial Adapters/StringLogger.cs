using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringLogger : SimpleSerialAdapter
{    
    private string currentMessage = "";

    public override bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        for (int i = 0; i < message.Length; ++i)
        {
            char c = (char)message[i];
            currentMessage += c;
            if (c == '\0')
            {
                OnMessageCompleted();
                bytesRead = i + 1;
                return true;
            }
        }

        bytesRead = message.Length;
        return false;
    }

    private void OnMessageCompleted()
    {
        Debug.Log(currentMessage);
        currentMessage = "";
    }
}
