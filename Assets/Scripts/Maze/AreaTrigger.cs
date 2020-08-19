using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public delegate void ExitTrigger(Collider exitee);
    public event ExitTrigger exitHandler;

    public delegate void EnterTrigger(Collider entree);
    public event EnterTrigger enterHandler;

    private Collider theCollider;
    
    void Start()
    {
        theCollider = GetComponent<Collider>();
        theCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {   
        enterHandler?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        exitHandler?.Invoke(other);
    }
}
