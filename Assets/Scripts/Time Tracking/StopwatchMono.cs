using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchMono : MonoBehaviour
{
    private Stopwatch stopwatch;
    public float CurrentTime { get=> stopwatch.Time; }

    private void Awake()
    {
        this.stopwatch = new Stopwatch();
    }

    public void Begin()
    {
        stopwatch.Start();
    }

    public void Reset()
    {
        stopwatch.Reset();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    private void FixedUpdate()
    {
        stopwatch.Tick(Time.deltaTime);
    }
}
