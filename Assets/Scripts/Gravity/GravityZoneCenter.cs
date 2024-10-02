using System.Collections.Generic;
using UnityEngine;

public class GravityZoneCenter : GravityZone
{
    
    public override Vector3 GetGravityDirection(ObjectGravity _gravityBody)
    {
        return (transform.position - _gravityBody.transform.position).normalized;
    }
}
