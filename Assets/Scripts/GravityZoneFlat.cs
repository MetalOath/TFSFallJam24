using UnityEngine;

public class GravityZoneFlat : GravityZone
{
    public override Vector3 GetGravityDirection(ObjectGravity _gravityBody)
    {
        return -transform.up;
    }
}