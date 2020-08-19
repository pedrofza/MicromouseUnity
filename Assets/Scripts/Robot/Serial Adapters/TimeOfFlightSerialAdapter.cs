using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    
public class TimeOfFlightSerialAdapter : SimpleSerialAdapterForComponent<TimeOfFlight>
{
    [SerializeField] private int numberOfBits;

    private void OnEnable()
    {
        Component.NewMeasurement += NewDistanceMeasurementCallback;
    }

    private void OnDisable()
    {
        Component.NewMeasurement -= NewDistanceMeasurementCallback;
    }

    public void NewDistanceMeasurementCallback(TimeOfFlight sender, DistanceMeasurement args)
    {
        float measuredDistance = args.Distance;
        float maxDistance = sender.Specification.Range;
        float ratio = measuredDistance / maxDistance;

        int maxCounts = MyMath.MaximumValueRepresentedByQuantityOfBits(numberOfBits);

        uint distanceCounts = (uint)Mathf.Round(ratio * maxCounts);
        
        byte[] countsAsBytes = BitConverter.GetBytes(distanceCounts);
        if(!BitConverter.IsLittleEndian)
        {
            Array.Reverse(countsAsBytes);
        }
        
        int numberOfBytes = (int)Mathf.Ceil(numberOfBits / 8.0f);
        byte[] countsAsBytesTrimmed = new byte[numberOfBytes];
        Buffer.BlockCopy(countsAsBytes, 0, countsAsBytesTrimmed, 0, numberOfBytes);
        OnNewOutputData(countsAsBytesTrimmed);
    }
}
