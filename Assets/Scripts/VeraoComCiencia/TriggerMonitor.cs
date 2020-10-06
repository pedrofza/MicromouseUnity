using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMonitor : MonoBehaviour
{
    [SerializeField] private UnityEvent objectEntered;
    [SerializeField] private UnityEvent objectExited;
    
    private void OnTriggerEnter(Collider other)
    {
        objectEntered.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        objectExited.Invoke();
    }
}
