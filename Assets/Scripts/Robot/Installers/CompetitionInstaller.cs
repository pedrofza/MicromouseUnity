using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CompetitionInstaller : MonoInstaller
{
    [SerializeField] private GameObject robotPrefab;

    public override void InstallBindings()
    {
        Container
            .Bind<Transform>()
            .FromComponentInNewPrefab(robotPrefab)
            .AsSingle()
            .OnInstantiated<Transform>((ctx, foo) => foo.parent=null)
        ;
    }
}
