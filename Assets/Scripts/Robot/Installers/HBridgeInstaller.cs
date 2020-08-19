using UnityEngine;
using Zenject;

public class HBridgeInstaller : MonoInstaller
{
    [SerializeField] private VoltageSourceExposer voltageSource;

    public override void InstallBindings()
    {
        HBridge hBridge = new HBridge();
        IVoltageSource vs = voltageSource.voltageSource();
        hBridge.SetVoltageSource(vs);
        Container
            .BindInterfacesAndSelfTo<HBridge>() // TODO: BindInterfacesTo<HBridge>();
            .FromInstance(hBridge)
            .AsSingle()
        ;
    }
}