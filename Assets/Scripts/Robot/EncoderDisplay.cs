using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EncoderDisplay : MonoBehaviour
{
    private IncrementalEncoder encoder;

    [Inject]
    public void Construct(IncrementalEncoder encoder)
    {
        this.encoder = encoder;
        encoder.NewMeasurement += NewCountsCallback;
    }

    private void NewCountsCallback(IncrementalEncoder sender, EncoderMeasurement args)
    {
        int counts = args.Counts;
        Debug.Log(counts);
    }
}
