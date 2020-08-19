using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEditor;

public enum DrawnRayOrigin
{
    Follow, Static
}

public class TimeOfFlightGizmo : MonoBehaviour
{
    private static Material lineMaterial;
    private static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
    
    private static readonly Color otherColor = Color.blue;

    [SerializeField] private DrawnRayOrigin rayLocation;
    [SerializeField] private Color hitColor = Color.green;
    [SerializeField] private Color missColor = Color.red;
    [SerializeField] private bool drawRay;
    [SerializeField] private bool drawHit;
    [SerializeField] private float hitSize;
    
    private TimeOfFlight tof;
    
    [Inject]
    private void Construct(TimeOfFlight tof)
    {
        this.tof = tof;
    }

    private Color GetLineColor(MeasurementStatus status)
    {
        switch (status)
        {
            case MeasurementStatus.success:
                return hitColor;
                break;
            case MeasurementStatus.timeout:
                return missColor;
                break;
            default:
                return otherColor;
                break;
        }
    }

    private bool IsRenderable()
    {
        if (!drawRay && !drawHit)
        {
            return false;
        }

        return true;
    }

    private Vector3 GetRenderingOrigin(DistanceMeasurement measurement)
    {
        switch(rayLocation)
        {   
            case DrawnRayOrigin.Follow:
                return tof.Position;
                break;
            case DrawnRayOrigin.Static:    
                return measurement.Origin;
                break;
            default:
                return measurement.Origin;
                break;
        }
    }

    private Quaternion GetRenderingRotation(DistanceMeasurement measurement)
    {   
        /* C#8.0 switch expression */
        // Vector3 rayOrigin = rayLocation switch
        // {
        //     DrawnRayOrigin.Follow => tof.Rotation * measurement.Rotation,
        //     DrawnRayOrigin.Static => measurement.Direction
        // };
        // return rayOrigin;

        switch(rayLocation)
        {   
            case DrawnRayOrigin.Follow:
                return tof.Rotation * measurement.FoViewDif;
                break;
            case DrawnRayOrigin.Static:    
                return measurement.Rotation * measurement.FoViewDif;
                break;
            default:
                return measurement.Rotation * measurement.FoViewDif;
                break;
        }
    }

    private Matrix4x4 CreateGLMultMatrix(DistanceMeasurement measurement)
    {
        Vector3 renderOrigin = GetRenderingOrigin(measurement);
        Quaternion renderRotation = GetRenderingRotation(measurement);
        Matrix4x4 m = Matrix4x4.TRS(renderOrigin, renderRotation, Vector3.one);
        return m;
    }

    private void OnRenderObject()
    {
        if (!IsRenderable())
        {
            return;
        }

        DistanceMeasurement measurement = tof.Measurement;
        if (measurement == null)
        {
            return;
        }

        Color lineColor = GetLineColor(measurement.Status);

        CreateLineMaterial();
        GL.PushMatrix();
        lineMaterial.SetPass(0);

        GL.MultMatrix(CreateGLMultMatrix(measurement));
        
        if (drawRay)
        {
            GL.Begin(GL.LINES);
            GL.Color(lineColor);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, measurement.Distance);
            GL.End();
        }

        if (drawHit)
        {
            GL.Begin(GL.TRIANGLES);
            GL.Color(lineColor);
            GL.Vertex3(0.0f, 2 * hitSize / 3, measurement.Distance);
            GL.Vertex3(hitSize / 2, -hitSize / 3, measurement.Distance);
            GL.Vertex3(-hitSize / 2, -hitSize / 3, measurement.Distance);
            GL.End();
        }

        GL.PopMatrix();
    }
}

