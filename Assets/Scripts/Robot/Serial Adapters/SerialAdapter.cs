using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public interface IHaveSerialId
{
    byte Id { get; }
}

public interface IReceiveSerialMessages : IHaveSerialId
{
    bool ReceiveMessage(byte[] message, out int bytesRead);
}

public class OutputDataArgs
{
    public byte[] Data { get; set; }
}

public delegate void SerialSenderEventHandler<T, U>(T sender, U message);
public interface ISendSerialMessages : IHaveSerialId
{
    event SerialSenderEventHandler<ISendSerialMessages, OutputDataArgs> NewOutputData;
}

public abstract class SimpleSerialAdapter : MonoBehaviour, IReceiveSerialMessages, ISendSerialMessages
{
    [SerializeField] private byte id;
    public byte Id { get => id; }
    
    protected SerialOutputBuffer sob;
    protected SerialInputBuffer sib;
    protected RobotSerialBus SerialBus {get; set; }

    [Inject]
    private void OutBufferInject(SerialOutputBuffer sob)
    {
        this.sob = sob;
        sob.RegisterSender(this);
    }

    [Inject]
    private void InBufferInject(SerialInputBuffer sib)
    {
        this.sib = sib;
        sib.RegisterReceiver(this);
    }

    [Inject]
    private void BusInject(RobotSerialBus bus)
    {
        SerialBus = bus;
    }


    public virtual bool ReceiveMessage(byte[] message, out int bytesRead)
    {
        bytesRead = 0;
        return true;
    }

    public event SerialSenderEventHandler<ISendSerialMessages, OutputDataArgs> NewOutputData;
    protected void OnNewOutputData(byte[] data)
    {
        var handler = NewOutputData;
        handler?.Invoke(this, new OutputDataArgs { Data = data });
    }
}

public class SimpleSerialAdapterForComponent<T> : SimpleSerialAdapter
{
    protected T Component { get; set; }

    [Inject]
    private void Construct(T component)
    {
        Component = component;
    }
}