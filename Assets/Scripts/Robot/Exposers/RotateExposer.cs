using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RotateExposer : MonoBehaviour
{
    [Inject] private IRotate rotate;
    public IRotate Get()
    {
        return rotate;
    }
}
