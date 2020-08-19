using UnityEngine;
using Zenject;

public class RobotInstaller : MonoInstaller
{
    private void InitExecutionOrder()
    {
        // Container.BindExecutionOrder<SerialInputBuffer>(-100); // Now only uses separate thread
        Container.BindExecutionOrder<SerialOutputBuffer>(100);
    }

    public override void InstallBindings()
    {
        InitExecutionOrder();
        
        Container.BindInterfacesAndSelfTo<SerialInputBuffer>()
            .AsSingle()
        ;

        Container.BindInterfacesAndSelfTo<SerialOutputBuffer>()
            .AsSingle()
        ;

        Container
            .BindInterfacesAndSelfTo<RobotSerialBus>() //TODO: .BindInterfacesTo<RobotSerialBus>()
            .AsSingle()
        ;
    }
}