using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TimeOfFlight : IFixedTickable
{
    private static readonly SharpNeat.Utility.FastRandom fr;
    private static readonly SharpNeat.Utility.ZigguratGaussianSampler zgs;

    static TimeOfFlight() {
        fr = new SharpNeat.Utility.FastRandom();
        zgs = new SharpNeat.Utility.ZigguratGaussianSampler(fr);
    }
    
    private Transform SensorTransform { get; set; }
    private TimeTracker newMeasurementDelay;
    
    public Vector3 Position { get => SensorTransform.position; }
    public Quaternion Rotation { get => SensorTransform.rotation; }
    public Vector3 Heading { get => SensorTransform.forward; }

    public TimeOfFlightSpecification Specification { get; private set; }

    public DistanceMeasurement Measurement {get; private set;}


    public TimeOfFlight(TimeOfFlightSpecification specification, Transform transform)
    {
        Specification = specification;
        SensorTransform = transform;
        this.newMeasurementDelay = new TimeTracker(Specification.UpdateTime);
    }

    public void FixedTick()
    {
        newMeasurementDelay.Update(Time.deltaTime);
        if(newMeasurementDelay.check())
        {
            PerformMeasurement();
        }
    }

    private void PerformMeasurement()
    {
        DistanceMeasurement measurement = MeasureDistanceSimple();
        OnNewDistanceMeasurement(measurement);
    }

    private Vector3 ApplyDirectionNoise(Vector3 heading)
    {
        Quaternion deviationFromCenterGaussianNoise = Quaternion.AngleAxis((float)zgs.NextSample(0.0f, 2 * Specification.FoView), SensorTransform.right);
        Quaternion rotateAboutCenterUniformNoise = Quaternion.AngleAxis((float)fr.NextDouble() * 360f, heading);
        Vector3 rayDirection = rotateAboutCenterUniformNoise * deviationFromCenterGaussianNoise * heading;
        return rayDirection;
    }

    private static bool SensorRaycast(Vector3 position, Vector3 direction, out RaycastHit hitInfo, float raycastLength)
    {
        return Physics.Raycast(
            position,
            direction,
            out hitInfo,
            raycastLength,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );
    }

    protected virtual DistanceMeasurement MeasureDistanceSimple()
    {
        Vector3 rayDirection = Heading;
        float raycastLength = Mathf.Infinity;

        RaycastHit hitInfo;
        bool didHit = SensorRaycast(Position, rayDirection, out hitInfo, raycastLength);

        float actualDistance = hitInfo.collider ? hitInfo.distance : raycastLength;
        float measuredDistance = Mathf.Clamp(actualDistance, 0, actualDistance);
        if(didHit)
        {
            measuredDistance = (float)zgs.NextSample(measuredDistance, Specification.CoVar * measuredDistance);
        }

        measuredDistance = Mathf.Clamp(measuredDistance, 0, Specification.Range);

        MeasurementStatus status = 
            measuredDistance < Specification.Range ? 
            MeasurementStatus.success : 
            MeasurementStatus.timeout
        ;

        DistanceMeasurement measurement = new DistanceMeasurement(
            Position,
            Rotation,
            Quaternion.identity,
            measuredDistance,
            status, 
            hitInfo.point
        );
        return measurement;
    }

    protected virtual DistanceMeasurement MeasureDistance()
    {
        Vector3 heading = Heading;
        Vector3 rayDirection = ApplyDirectionNoise(heading);
        float raycastLength = Mathf.Infinity;

        RaycastHit hitInfo;
        bool didHit = SensorRaycast(Position, rayDirection, out hitInfo, raycastLength);

        float actualDistance = hitInfo.collider ? hitInfo.distance : raycastLength;
        float measuredDistance = Mathf.Clamp(actualDistance, Specification.BlindSpot, actualDistance);
        if(didHit)
        {
            measuredDistance = (float)zgs.NextSample(measuredDistance, Specification.CoVar * measuredDistance);
        }

        measuredDistance = Mathf.Clamp(measuredDistance, 0, Specification.Range);

        MeasurementStatus status = 
            measuredDistance < Specification.Range ? 
            MeasurementStatus.success : 
            MeasurementStatus.timeout
        ;

        DistanceMeasurement measurement = new DistanceMeasurement(
            Position,
            Rotation,
            Quaternion.FromToRotation(heading, rayDirection),
            measuredDistance,
            status, 
            hitInfo.point
        );
        return measurement;
    }

    public delegate void DistanceMeasurementEventHandler<T, U>(T sender, U eventArgs);
    public event DistanceMeasurementEventHandler<TimeOfFlight, DistanceMeasurement> NewMeasurement;

    private void OnNewDistanceMeasurement(DistanceMeasurement measurement)
    {
        Measurement = measurement;
        var handler = NewMeasurement;
        handler?.Invoke(this, measurement);
    }
}
