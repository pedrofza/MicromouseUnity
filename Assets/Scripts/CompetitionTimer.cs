using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitionTimer
{
    private Countdown globalTimer;
    private Stopwatch lapTimer;
    
    public CompetitionTimer(float maximumTime)
    {
        globalTimer = new Countdown(maximumTime);
        lapTimer = new Stopwatch();
    }

    public void StartGlobal()
    {
        globalTimer.Start();
    }

    public void StartRun()
    {
        lapTimer.Start();
    }

    public void EndRun()
    {
        lapTimer.Stop();
    }

    public void ResetRun()
    {
        lapTimer.Reset();
    }

    public void Tick(float deltaTime)
    {
        globalTimer.Tick(deltaTime);
        lapTimer.Tick(deltaTime);
    }

    public float GlobalTime()
    {
        return globalTimer.Time;
    }

    public bool HasExpired()
    {
        return globalTimer.HasExpired;
    }

    public float LapTime()
    {
        return lapTimer.Time;
    }
}