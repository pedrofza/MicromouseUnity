using UnityEngine;
using Zenject;

public class BatteriesInstaller : MonoInstaller
{
    [SerializeField] private float voltage;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesTo<Battery>()
            .AsSingle()
            .WithArguments(voltage)
        ;
    }
}
