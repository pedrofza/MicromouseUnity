using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransformMutability : MonoBehaviour
{
    Transform t;

    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(t.position);
    }
}
