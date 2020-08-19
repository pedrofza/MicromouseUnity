using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncoderSpecification", menuName = "Specifications/Encoder")]
public class EncoderSpecification : ScriptableObject
{
    [SerializeField] private float pulsesPerRevolution;

    public float PPR { get=> pulsesPerRevolution; }
}
