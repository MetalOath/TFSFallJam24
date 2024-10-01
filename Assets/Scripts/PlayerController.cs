using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // SerializeField allows private fields to be editable in the Unity Inspector
    [SerializeField] private LayerMask _groundMask; // Mask for detecting ground layers during ground check
    [SerializeField] private Transform _groundCheck; // Transform for the ground check position
    [SerializeField] private Transform _cam; // Transform for the camera (if needed for future camera control)
    [SerializeField] private Animator _animator; // Animator component to handle player animations

    private float _groundCheckRadius = 0.3f; // Radius for the ground detection sphere
    private float _speed = 8; // Player movement speed
    private float _turnSpeed = 1500f; // Speed of the player turning
    private float _jumpForce = 500f; // Force applied when the player jumps

    private Rigidbody _rigidbody; // Rigidbody for physics-based movement
    private Vector3 _direction; // Direction of player movement

    private ObjectGravity _gravityBody; // Reference to ObjectGravity component for gravity direction

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component attached to the player
        _rigidbody = transform.GetComponent<Rigidbody>();
        // Get the ObjectGravity component (for handling custom gravity)
        _gravityBody = transform.GetComponent<ObjectGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        // Capture input from horizontal and vertical axes and normalize it to get movement direction
        _direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Check if the player is grounded by casting a sphere at the ground check position
        bool isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        // Update the Animator with the jumping state (if the player is in the air, 'isJumping' will be true)
        // _animator.SetBool("isJumping", !isGrounded);

        // Handle jumping: if the Space key is pressed and the player is grounded, apply a jump force
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Apply a force in the opposite direction of gravity to make the player jump
            _rigidbody.AddForce(-_gravityBody.GravityDirection * _jumpForce, ForceMode.Impulse);
        }
    }

    // FixedUpdate is called at a fixed time interval and is used for physics calculations
    void FixedUpdate()
    {
        // Check if the player is moving (i.e., if input magnitude is above a small threshold)
        bool isRunning = _direction.magnitude > 0.1f;

        if (isRunning)
        {
            // Move the player forward based on their current facing direction and input
            Vector3 direction = transform.forward * _direction.z;
            _rigidbody.MovePosition(_rigidbody.position + direction * (_speed * Time.fixedDeltaTime));

            // Calculate a rotation based on input and the turn speed
            Quaternion rightDirection = Quaternion.Euler(0f, _direction.x * (_turnSpeed * Time.fixedDeltaTime), 0f);

            // Smoothly interpolate between the current rotation and the target rotation
            Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, _rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f);

            // Apply the new rotation to the player's Rigidbody
            _rigidbody.MoveRotation(newRotation);
        }

        // Update the Animator with the running state (true if the player is moving, false if idle)
        // _animator.SetBool("isRunning", isRunning);
    }
}
