using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "All Fires")]
public class SO_FireSpecs : ScriptableObject
{
    [Header("Fire Specs")]
    [SerializeField] public float FireLerpDuration;
    [SerializeField] public Vector2 RndEmissionRange;
    [SerializeField] public Vector2 RndColorRange;
    [SerializeField] public Vector2 RndColorIntervalRange;
    [SerializeField] public Vector2 RndEmissionIntervalRange;
    [SerializeField] public Material FireMaterial;
    [SerializeField] public float CameraRenderDistance;
}
