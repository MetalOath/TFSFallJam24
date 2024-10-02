using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Ensures that a Rigidbody component is attached to this GameObject
[RequireComponent(typeof(Rigidbody))]
public class ObjectGravity : MonoBehaviour
{
    // A constant value for the gravitational force applied to this body
    private static float GRAVITY_FORCE = 800;

    // A property to get the current gravity direction affecting the object
    public Vector3 GravityDirection
    {
        get
        {
            // If no gravity areas are affecting the object, return zero (no gravity)
            if (_gravityAreas == null || _gravityAreas.Count == 0) return Vector3.zero;

            // Sort the gravity areas based on their priority, lowest to highest
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));

            // Return the gravity direction from the highest priority gravity area, normalized
            return _gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody _rigidbody; // Rigidbody component to apply physics
    private List<GravityZone> _gravityAreas; // List of GravityArea objects affecting this body

    // Start is called before the first frame update
    void Awake()
    {
        // Get the Rigidbody component attached to this GameObject
        _rigidbody = transform.GetComponent<Rigidbody>();

        // Initialize the list to keep track of gravity areas affecting the object
        _gravityAreas = new List<GravityZone>();
    }

    // FixedUpdate is called at a fixed time interval, ideal for physics calculations
    void FixedUpdate()
    {
        // Apply a force in the direction of gravity, scaled by the gravity force and delta time
        _rigidbody.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode.Acceleration);

        // Calculate the required rotation to align the object's up direction with the opposite of the gravity direction
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);

        // Smoothly interpolate the object's current rotation towards the new desired rotation
        Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * 3f);

        // Apply the new rotation to the Rigidbody
        _rigidbody.MoveRotation(newRotation);
    }

    // Adds a gravity area to the list of areas affecting this object
    public void AddGravityArea(GravityZone gravityArea)
    {
        _gravityAreas.Add(gravityArea);
    }

    // Removes a gravity area from the list of areas affecting this object
    public void RemoveGravityArea(GravityZone gravityArea)
    {
        _gravityAreas.Remove(gravityArea);
    }
}
