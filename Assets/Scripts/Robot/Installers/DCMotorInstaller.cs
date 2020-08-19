using UnityEngine;
using Zenject;

public class DCMotorInstaller : MonoInstaller
{
    [SerializeField] private DCMotorSpecification specs;
    [SerializeField] private VoltageSourceExposer voltageSource;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Vector3 axis;

    public override void InstallBindings()
    {
        DCMotor motor = new DCMotor(specs);
        motor.SetTarget(body, axis);
        motor.SetVoltageSource(voltageSource.voltageSource());
        Container
            .BindInterfacesAndSelfTo<DCMotor>()
            .FromInstance(motor)
            .AsSingle()
            .WithArguments(body, axis)
        ;
    }
}