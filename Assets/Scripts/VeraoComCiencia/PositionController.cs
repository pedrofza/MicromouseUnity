using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;


public class PositionController : MonoBehaviour
{
    [SerializeField] private UnityEvent completedEvent;

    [SerializeField] private Collider robotBody;
    [SerializeField] private float goalDistance;
    [SerializeField] private float tolerance;
    [SerializeField] private float timeToMantain;

    [SerializeField] private TMP_Text currentDistanceText;
    
    private float timeWithinArea;
    private float currentPosition;

    private void Awake()
    {
        timeWithinArea = 0.0f;
    }
    
    private void FixedUpdate()
    {
        if (IsOnGoal())
        {
            timeWithinArea += Time.deltaTime;
        }
        else
        {
            timeWithinArea = 0.0f;
        }

        if (CheckWinCondition())
        {
            completedEvent.Invoke();
            Destroy(this);
        }
    }

    private void Update()
    {
        currentDistanceText.text = $"Distância: {currentPosition:F3} m";
        if (timeWithinArea == 0)
        {
            currentDistanceText.color = Color.red;
        }
        else
        {
            currentDistanceText.color = Color.green;
        }
    }

    private bool IsOnGoal()
    {
        float x = robotBody.transform.position.x;
        currentPosition = x;
        float minGoal = goalDistance - tolerance;
        float maxGoal = goalDistance + tolerance;
        return minGoal <= x && x <= maxGoal;
    }

    private bool CheckWinCondition()
    {
        return timeWithinArea >= timeToMantain;
    }

    private void ResetTime()
    {
        timeWithinArea = 0.0f;
    }
}
