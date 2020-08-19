using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 LocalAngularVelocity(this Rigidbody rb)
    {
        return rb.transform.InverseTransformDirection(rb.angularVelocity);
    }

    public static float NextGaussian(this System.Random r, float mu = 0, float sigma = 1)
    {
        float u1 = (float) r.NextDouble();
        float u2 = (float) r.NextDouble();
        float rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float rand_normal = mu + sigma * rand_std_normal;
        return rand_normal;
    }
}
