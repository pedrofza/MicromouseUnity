using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class IncrementalEncoderSerialAdapter : SimpleSerialAdapterForComponent<IncrementalEncoder>
{
    [SerializeField] private int numberOfBytes;
    [SerializeField] private float sendingDelay;

    private TimeTracker newSendDelay;
    private int countsAcc;

    private void Awake()
    {
        newSendDelay = new TimeTracker(sendingDelay);
        countsAcc = 0;
    }
    
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
        countsAcc += counts;
        if(newSendDelay.check())
        {
            byte[] countsAsBytes = BitConverter.GetBytes(countsAcc);
            if(!BitConverter.IsLittleEndian)
            {
                Array.Reverse(countsAsBytes);
            }
            byte[] countsAsBytesTrimmed = new byte[numberOfBytes];
            Buffer.BlockCopy(countsAsBytes, 0, countsAsBytesTrimmed, 0, numberOfBytes);
            OnNewOutputData(countsAsBytesTrimmed);
            countsAcc = 0;
        }
    }

    private void FixedUpdate()
    {
        newSendDelay.Update(Time.deltaTime);
    }
}
