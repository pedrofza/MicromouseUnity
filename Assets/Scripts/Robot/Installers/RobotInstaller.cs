using UnityEngine;
using Zenject;

public class RobotInstaller : MonoInstaller
{
    private void InitExecutionOrder()
    {
        /* 
        * The following line is needed only when SerialInputBuffer is updated in a loop. 
        * In the current implementation, it runs as asynchronously
        // Container.BindExecutionOrder<SerialInputBuffer>(-100);
        */
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