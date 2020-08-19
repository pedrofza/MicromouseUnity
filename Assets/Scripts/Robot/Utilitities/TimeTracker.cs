using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracker
{
    private float elapsedTime;
    private float targetTime;

    public TimeTracker(float targetTime)
    {
        this.targetTime = targetTime;
        this.elapsedTime = 0.0f;
    }

    public void Update(float elapsedTime)
    {
        this.elapsedTime += elapsedTime;
    }

    public bool check()
    {
        if (elapsedTime >= targetTime)
        {
            elapsedTime %= targetTime;
            return true;
        }
        return false;
    }
}
