using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RigidbodyInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody rb;
    
    public override void InstallBindings()
    {
        RigidbodyAdapter rbAdapter = new RigidbodyAdapter(rb);
        Container
            .Bind<RigidbodyAdapter>()
            .FromInstance(rbAdapter)
            .AsSingle()
        ;
    }
}
