using System.Collections.Generic;
using UnityEngine;

public class GravityZonePlanetInverse : GravityZone
{
    [SerializeField] private Vector3 _center;

    
    public override Vector3 GetGravityDirection(ObjectGravity _gravityBody)
    {
        return (_gravityBody.transform.position - _center).normalized;
    }
}
