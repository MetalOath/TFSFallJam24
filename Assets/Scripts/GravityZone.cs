using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class GravityZone : MonoBehaviour
{
    [SerializeField] private int _priority;
    public int Priority => _priority;
    
    void Start()
    {
        transform.GetComponent<Collider>().isTrigger = true;
    }
    
    public abstract Vector3 GetGravityDirection(ObjectGravity _gravityBody);
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ObjectGravity gravityBody))
        {
            gravityBody.AddGravityArea(this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ObjectGravity gravityBody))
        {
            gravityBody.RemoveGravityArea(this);
        }
    }
}