using System.Collections.Generic;
using UnityEngine;

public class GravityZonePlanet : GravityZone
{
    [SerializeField] private Vector3 _center;
    [SerializeField] private bool overrideCenter;

    private void Awake()
    {
        if (!overrideCenter) { }
        else
            _center = transform.parent.position;
    }

    public override Vector3 GetGravityDirection(ObjectGravity _gravityBody)
    {
        return (_center - _gravityBody.transform.position).normalized;
    }
}
