using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TimeOfFlight", menuName = "Specifications/TimeOfFlight")]
public class TimeOfFlightSpecification : ScriptableObject
{
    [SerializeField] private float range;
    [SerializeField] private float blindSpot;
    [SerializeField] private float coefficientOfVariance;
    [SerializeField] private float fieldOfView;
    [SerializeField] private float updateTime;

    public float Range { get => range; }
    public float BlindSpot { get => blindSpot; }
    public float CoVar { get => coefficientOfVariance; }
    public float FoView { get => fieldOfView; }
    public float UpdateTime { get => updateTime; }
}
