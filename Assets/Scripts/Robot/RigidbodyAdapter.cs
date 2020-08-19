using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRigidbodyAdapter : IRotate
{

}

public class RigidbodyAdapter : IRigidbodyAdapter
{
    private Rigidbody rb;

    public RigidbodyAdapter(Rigidbody rb)
    {
        this.rb = rb;
    }

    public Vector3 GetAngularVelocity()
    {
        return rb.LocalAngularVelocity();
    }
}
