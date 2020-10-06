using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Zenject;
using UnityEngine.SceneManagement;



public class CompetitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private MazePhysicalSpecification mazeSpecs;

    [SerializeField] private TMP_Text globalTimerField;
    [SerializeField] private TMP_Text lapTimerField;
    [SerializeField] private PhysicMaterial mazeMaterial;
    [SerializeField] private GameObject mazeWallPrefab;
    [SerializeField] private GameObject mazeFloorPrefab;

    [Inject] private Transform robotPrefab;

    private CompetitionTimer compTimer;
    private Maze maze;

    private void Awake()
    {
        compTimer = new CompetitionTimer(60 * 10);
    }

    private void OnEnable()
    {
        if (!PopulateMazeSelection.selectedAsset)
        {
            Debug.Log("Asset was null");
            return;
        }
        maze = new Maze(PopulateMazeSelection.selectedAsset, mazeSpecs, mazeMaterial, mazeWallPrefab, mazeFloorPrefab);
        maze.Build();
        robotPrefab.transform.position = spawnPosition + maze.StartingPosition();
    }
    
    private void FixedUpdate()
    {
        compTimer.Tick(Time.deltaTime);
    }

    public void Update()
    {
        globalTimerField.SetText(TimeSpan.FromSeconds(compTimer.GlobalTime()).ToString(@"mm\:ss\:fff"));
        lapTimerField.SetText(TimeSpan.FromSeconds(compTimer.LapTime()).ToString(@"mm\:ss\:fff"));
        if (compTimer.HasExpired())
        {
            globalTimerField.color = Color.red;
            if (compTimer.HasExpired())
            {
                lapTimerField.color = Color.red;
            }
        }
    }
}
