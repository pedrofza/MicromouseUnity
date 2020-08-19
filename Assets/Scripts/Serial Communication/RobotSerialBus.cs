using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading;
using Zenject;
using System;

public class RobotSerialBus : ILateDisposable
{
    private MySerialPort serialPort; //TODO: Inject generic
    private ConcurrentQueue<byte> dataBuffer;
    private Dictionary<byte, IReceiveSerialMessages> messageRecipients;
    private IReceiveSerialMessages currentRecipient;

    private CancellationTokenSource cts;

    public RobotSerialBus()
    {
        serialPort = new MySerialPort();
        dataBuffer = new ConcurrentQueue<byte>();
        messageRecipients = new Dictionary<byte, IReceiveSerialMessages>();
        this.currentRecipient = null;
        cts = new CancellationTokenSource();
    }

    public void LateDispose()
    {
        Disconnect();
    }

    public void Configure(string portName, int baudRate)
    {
        if (serialPort.IsOpen)
        {
            Debug.Log("Can't configure while port is open");
            return;
        }

        serialPort.PortName = portName;
        serialPort.BaudRate = baudRate;
    }

    public void Connect()
    {
        serialPort.Open();
        serialPort.ContinuousRead(cts.Token);
    }

    public void Disconnect()
    {
        cts.Cancel();
        serialPort.Close();
        Debug.Log("Serial Port Closed");
    }

    public bool IsConnected { get => serialPort.IsOpen; }    

    public event EventHandler<MySerialPort.DataReceivedArgs> BusDataReceived
    {
        add { this.serialPort.DataReceived += value; }
        remove { this.serialPort.DataReceived += value; }
    }

    public void SendData(byte[] data, int qty)
    {
        if (IsConnected)
        {   
            serialPort.BaseStream.Write(data, 0, qty);
        }
    }


}
