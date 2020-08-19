using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class IncrementalEncoderSerialAdapter : SimpleSerialAdapterForComponent<IncrementalEncoder>
{
    [SerializeField] private int numberOfBytes;

    private void OnEnable()
    {
        Component.NewMeasurement += NewCountsCallback;
    }

    private void OnDisable()
    {
        Component.NewMeasurement -= NewCountsCallback;
    }

    private void NewCountsCallback(IncrementalEncoder sender, EncoderMeasurement args)
    {
        int counts = args.Counts;
        byte[] countsAsBytes = BitConverter.GetBytes(counts);
        if(!BitConverter.IsLittleEndian)
        {
            Array.Reverse(countsAsBytes);
        }
        byte[] countsAsBytesTrimmed = new byte[numberOfBytes];
        Buffer.BlockCopy(countsAsBytes, 0, countsAsBytesTrimmed, 0, numberOfBytes);
        OnNewOutputData(countsAsBytesTrimmed);
    }
}
