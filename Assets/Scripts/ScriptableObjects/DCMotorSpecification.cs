using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DCMotorSpecification", menuName = "Specifications/DC Motor")]

public class DCMotorSpecification : ScriptableObject
{
    [SerializeField] private float ratedVoltage;
    [SerializeField] private float gearRatio;
    [SerializeField] private float torqueConstant;
    [SerializeField] private float resistance;
    [SerializeField] private float inductance;

    public float RatedVoltage { get => ratedVoltage; }
    public float GearRatio { get => gearRatio; }
    public float Kt { get => torqueConstant; }
    public float R { get => resistance; }
    public float L { get => inductance; }
}
