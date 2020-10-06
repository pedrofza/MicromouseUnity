using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CountdownMono : MonoBehaviour
{
    [SerializeField] private float initialTime;
    [SerializeField] private UnityEvent timeout;

    private Countdown countdown;

    private void Awake()
    {
        this.countdown = new Countdown(initialTime);
        this.countdown.TimeoutEvent += RaiseTimeoutEvent;
    }

    private void FixedUpdate()
    {
        this.countdown.Tick(Time.deltaTime);
    }

    public void StartCountdown()
    {
        this.countdown.Start();
    }

    private void RaiseTimeoutEvent(object sender, EventArgs args)
    {
        timeout.Invoke();
    }

    public float TimeLeft { get => countdown.Time; }
    public bool HasExpired { get => countdown.HasExpired; }
}
