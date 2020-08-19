using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class SerialOutputBuffer : IFixedTickable
{
    private RobotSerialBus bus;
    private List<byte> outBuffer;
    private bool sendEnabled;
    public bool IsSendEnabled { get => sendEnabled; }

    private const int bufferSize = 4096;
    private byte[] buffer;

    public void EnableSend()
    {
        this.sendEnabled = true;
    }

    public void DisableSend()
    {
        this.sendEnabled = false;
    }

    [Inject]
    public SerialOutputBuffer(RobotSerialBus bus)
    {
        this.bus = bus;
        this.outBuffer = new List<byte>(bufferSize);
        this.buffer = new byte[bufferSize];
    }

    public void FixedTick() //should be AFTER every other FixedTick
    {
        outBuffer.CopyTo(buffer);
        bus.SendData(buffer, outBuffer.Count);
        outBuffer.Clear();
    }

    public void RegisterSender(ISendSerialMessages sender)
    {
        sender.NewOutputData += SerialSenderCallback;
    }

    public void UnregisterSender(ISendSerialMessages sender)
    {
        sender.NewOutputData -= SerialSenderCallback;
    }

    private void SerialSenderCallback(ISendSerialMessages sender, OutputDataArgs args)
    {
        outBuffer.Add(sender.Id);
        outBuffer.AddRange(args.Data);
    }
}
