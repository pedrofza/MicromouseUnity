using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeasurementStatus
{
    success,
    timeout
}

public class DistanceMeasurement
{
    public Vector3 Origin { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Quaternion FoViewDif { get; private set; }
    public float Distance { get; private set; }
    public MeasurementStatus Status { get; private set; }
    public Vector3 HitPoint { get; private set; }

    public DistanceMeasurement(Vector3 origin, Quaternion rotation, Quaternion fovDif, float distance, MeasurementStatus status, Vector3 hitPoint)
    {
        Origin = origin;
        Rotation = rotation;
        FoViewDif = fovDif;
        Distance = distance;
        Status = status;
        HitPoint = hitPoint;
    }
}

public interface ITimeOfFlight
{

}
