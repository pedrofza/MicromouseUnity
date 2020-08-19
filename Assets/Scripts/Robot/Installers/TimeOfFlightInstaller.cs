using UnityEngine;
using Zenject;

public class TimeOfFlightInstaller : MonoInstaller
{
    [SerializeField] private TimeOfFlightSpecification specs;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<TimeOfFlight>() //TODO: .BindInterfacesTo<TimeOfFlight>()
            .AsSingle()
            .WithArguments(specs, transform)
        ;
    }
}