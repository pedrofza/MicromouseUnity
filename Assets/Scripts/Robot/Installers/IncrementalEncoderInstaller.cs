using UnityEngine;
using Zenject;

public class IncrementalEncoderInstaller : MonoInstaller
{
    [SerializeField] private EncoderSpecification specs;
    [SerializeField] private Vector3 axis;
    [SerializeField] private RotateExposer attached;

    public override void InstallBindings()
    {
        IncrementalEncoder encoder = new IncrementalEncoder(specs);
        encoder.SetAttachedBody(attached.Get(), axis);
        Container
            .BindInterfacesAndSelfTo<IncrementalEncoder>() //TODO: .BindInterfacesTo<IncrementalEncoder>()
            .FromInstance(encoder)
            .AsSingle()
        ;
    }
}