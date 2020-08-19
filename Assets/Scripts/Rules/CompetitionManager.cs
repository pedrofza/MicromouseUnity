using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Zenject;

public class MyTimer
{
    private bool running;
    private float elapsedTime;
    private float expirationTime;

    public MyTimer()
    {
        this.running = false;
        this.elapsedTime = 0.0f;
    }

    public void Start()
    {
        this.running = true;
    }

    public void Reset()
    {
        this.elapsedTime = 0.0f;
    }

    public void Stop()
    {
        this.running = false;
    }

    public float CurrentTime()
    {
        return elapsedTime;
    }

    public void Update(float deltaTime)
    {
        if (running)
        {
            elapsedTime += deltaTime;
        }
    }
}

public class CompetitionTimer
{
    private MyTimer globalTimer;
    private MyTimer lapTimer;
    
    public CompetitionTimer()
    {
        globalTimer = new MyTimer();
        lapTimer = new MyTimer();
    }

    public void Start()
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

    public void Update(float deltaTime)
    {
        globalTimer.Update(deltaTime);
        lapTimer.Update(deltaTime);
    }

    public float GlobalTime()
    {
        return globalTimer.CurrentTime();
    }

    public float LapTime()
    {
        return lapTimer.CurrentTime();
    }
}

public class CompetitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private MazePhysicalSpecification mazeSpecs;
    [SerializeField] private TextAsset mazeDescriptionFile;
    
    [SerializeField] private TextMeshProUGUI globalTimerField;
    [SerializeField] private TextMeshProUGUI lapTimerField;
    [SerializeField] private PhysicMaterial mazeMaterial;

    [Inject] private Transform robotPrefab;

    private CompetitionTimer compTimer;
    private Maze maze;

    private void Awake()
    {
        maze = new Maze(mazeDescriptionFile, mazeSpecs, mazeMaterial);
        compTimer = new CompetitionTimer();
        maze.Build();
        robotPrefab.transform.position = spawnPosition + maze.StartingPosition();
    }
    
    private void FixedUpdate()
    {
        compTimer.Update(Time.deltaTime);
    }

    public void Update()
    {
        globalTimerField.SetText(TimeSpan.FromSeconds(compTimer.GlobalTime()).ToString(@"mm\:ss\:fff"));
        lapTimerField.SetText(TimeSpan.FromSeconds(compTimer.LapTime()).ToString(@"mm\:ss\:fff"));
    }
}
