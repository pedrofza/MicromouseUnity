using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Countdown
{
    public event EventHandler TimeoutEvent;

    private float initialTime;
    private float currentTime;
    private bool isRunning;

    public float Time { get => currentTime; }
    public bool HasExpired { get => Time <= 0.0f; }

    public Countdown(float time)
    {
        this.initialTime = time;
        Reset();
    }

    public void Start()
    {
        if (!isRunning)
        {
            currentTime = initialTime;
            isRunning = true;
        }
    }

    public void Reset()
    {
        this.currentTime = this.initialTime;
        this.isRunning = false;
    }

    public void Tick(float deltaTime)
    {
        if (isRunning)
        {
            currentTime -= deltaTime;
            if (currentTime <= 0.0f)
            {
                RaiseTimeoutEvent();
                currentTime = 0.0f;
                isRunning = false;
            }
        }
    }

    private void RaiseTimeoutEvent()
    {
        TimeoutEvent?.Invoke(this, EventArgs.Empty);
    }
}
