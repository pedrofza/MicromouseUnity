using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class MySerialPort : SerialPort
{
    public void ContinuousRead(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[4096];
        Action kickoffRead = null;
        kickoffRead = (Action)(() => BaseStream.BeginRead(
            buffer, 0, buffer.Length,
            delegate (IAsyncResult ar)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    int count = BaseStream.EndRead(ar);
                    byte[] dst = new byte[count];
                    Buffer.BlockCopy(buffer, 0, dst, 0, count);
                    OnDataReceived(dst);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("OptimizedSerialPort exception !");
                }
                kickoffRead();
            },
            null
        ));
        kickoffRead();
    }

    public delegate void DataReceivedEventHandler(object sender, DataReceivedArgs e);
    public new event EventHandler<DataReceivedArgs> DataReceived;

    public virtual void OnDataReceived(byte[] data)
    {
        var handler = DataReceived;
        handler?.Invoke(this,  new DataReceivedArgs { Data = data });
    }

    public class DataReceivedArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}

