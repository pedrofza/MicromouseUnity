using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncoderMeasurement
{
    public int Counts { get; private set; }
    public EncoderMeasurement(int counts)
    {
        Counts = counts;
    }
}

public interface IIncrementalEncoder
{
}
