using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

public class SerialInputBuffer
{
    private RobotSerialBus bus;
    private Queue<byte> dataBuffer;
    private Dictionary<byte, IReceiveSerialMessages> messageRecipients;
    private bool messageCompleted;
    private IReceiveSerialMessages currentRecipient;

    [Inject]
    public SerialInputBuffer(RobotSerialBus bus)
    {
        this.bus = bus;
        bus.BusDataReceived += OnDataReceived;
        dataBuffer = new Queue<byte>();
        messageRecipients = new Dictionary<byte, IReceiveSerialMessages>();
        this.messageCompleted = true;
        this.currentRecipient = null;
    }

    public void RegisterReceiver(IReceiveSerialMessages receiver)
    {
        byte key = receiver.Id;
        messageRecipients.Add(key, receiver);
    }

    public void UnregisterReceiver(IReceiveSerialMessages receiver)
    {
        byte key = receiver.Id;
        messageRecipients.Remove(key);
    }

    private void StoreDataInBuffer(byte[] data)
    {
        foreach (byte dataByte in data)
        {
            dataBuffer.Enqueue(dataByte);
        }
    }

    private void OnDataReceived(object sender, MySerialPort.DataReceivedArgs args)
    {
        StoreDataInBuffer(args.Data);

        do {
            if (messageCompleted)
            {
                byte inputId = dataBuffer.Dequeue();
                currentRecipient = messageRecipients[inputId];
            }
            byte[] message = dataBuffer.ToArray();
            int readBytes;
            messageCompleted = currentRecipient.ReceiveMessage(message, out readBytes);
            for (int i = 0; i < readBytes; i++)
            {
                dataBuffer.Dequeue();
            }
        } while (messageCompleted && dataBuffer.Count != 0);
    }
}