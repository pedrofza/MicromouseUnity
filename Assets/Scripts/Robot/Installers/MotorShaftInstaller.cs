using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MotorShaftInstaller : MonoInstaller
{
    [SerializeField] private bool isOutputShaft;
    [Inject] private DCMotor motor;

    public override void InstallBindings()
    {
        float reduction = isOutputShaft ? 1.0f : motor.Specification.GearRatio;
        Container
            .BindInterfacesAndSelfTo<MotorShaft>()
            .AsSingle()
            .WithArguments(motor, reduction)
        ;
    }
}
