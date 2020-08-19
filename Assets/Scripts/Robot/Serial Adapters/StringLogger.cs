// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class StringLogger : SimpleSerialAdapter
// {
//     public StringLogger(byte id) : base(id)
//     {
//     }
    
//     public override bool ReceiveMessage(byte[] message, out int bytesRead)
//     {
//         string stringMessage = "";
//         int strEndIndex = -1;
//         for (int i = 0; i < message.Length; ++i)
//         {
//             char c = (char)message[i];
//             if (c == '\0')
//             {
//                 strEndIndex = i;
//                 break;
//             }
//         }

//         if(strEndIndex == -1)
//         {
//             bytesRead = 0;
//             return false;
//         }

//         bytesRead = strEndIndex;

//         for(int i = 0; i < strEndIndex; ++i)
//         {
//             stringMessage += message[i];
//         }

//         Debug.Log(stringMessage);
//         return true;
//     }
// }
