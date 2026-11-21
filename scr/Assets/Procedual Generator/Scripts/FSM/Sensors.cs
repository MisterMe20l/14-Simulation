using UnityEngine;

public class Sensors
{
    private LayerMask targetMask;
    private float sensorRadius;
    private Transform selfPos;


    public Sensors(Transform _selfPos, float _sensorRadius, LayerMask _targetMask)
    {
        selfPos = _selfPos;
        sensorRadius = _sensorRadius;
        targetMask = _targetMask;
    }

    public bool TargetInside()
    {
        Collider[] colliderArr = Physics.OverlapSphere(selfPos.position, sensorRadius, targetMask);
        return colliderArr.Length > 0;
    }
}
