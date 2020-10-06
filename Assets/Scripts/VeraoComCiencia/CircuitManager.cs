using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CircuitManager : MonoBehaviour
{
    private bool isPrepared = false;
    private bool isLapRunning = false;
    private float previousBest = Mathf.Infinity;
    private Stopwatch stopwatch;

    [SerializeField] private TMP_Text tfLap;
    [SerializeField] private TMP_Text tfTime;
    [SerializeField] private TMP_Text tfBest;

    private void Awake()
    {
        stopwatch = new Stopwatch();
    }

    public void PrepareForRegistering()
    {
        isPrepared = true;
    }

    public void Register()
    {
        if(isPrepared)
        {
            StartNewLap();
        }
    }

    private int currentLap = 0;
    private void StartNewLap()
    {
        if(isLapRunning)
        {
            float lapTime = stopwatch.Time;
            if (lapTime <= previousBest)
            {
                previousBest = lapTime;
                tfBest.text = "Melhor " + TimeSpan.FromSeconds(previousBest).ToString(@"mm\:ss\:fff");
            }
        }
        currentLap++;
        stopwatch.Reset();
        stopwatch.Start();
        isLapRunning = true;
        isPrepared = false;
    }

    private void Update()
    {
        tfLap.text = $"Volta #{currentLap}";
        tfTime.text = "Tempo " + TimeSpan.FromSeconds(stopwatch.Time).ToString(@"mm\:ss\:fff");
    }

    private void FixedUpdate()
    {
        stopwatch.Tick(Time.deltaTime);
    }
}
