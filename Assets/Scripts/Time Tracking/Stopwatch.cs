using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch
{
    private float time;
    private bool isRunning;
    public bool IsRunning { get => isRunning; }
    public Stopwatch()
    {
        isRunning = false;
        time = 0.0f;
    }

    public void Tick(float deltaTime)
    {
        if (isRunning)
        {
            time += deltaTime;
        }
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Reset()
    {
        Stop();
        time = 0.0f;
    }

    public float Time
    {
        get => time;
    }
}
