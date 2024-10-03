using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _cam;
    [SerializeField] private Animator _animator;

    private float _groundCheckRadius = 0.3f;
    private float _speed = 8f;
    private float _jumpForce = 500f;

    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private ObjectGravity _gravityBody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gravityBody = GetComponent<ObjectGravity>();
    }

    void Update()
    {
        // Capture input
        _direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Ground check
        bool isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rigidbody.AddForce(-_gravityBody.GravityDirection * _jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        bool isRunning = _direction.magnitude > 0.1f;

        if (isRunning)
        {
            // Get gravity-up direction
            Vector3 gravityUp = -_gravityBody.GravityDirection.normalized;

            // Project camera directions onto the plane perpendicular to gravity
            Vector3 camUp = Vector3.ProjectOnPlane(_cam.up, gravityUp).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(_cam.right, gravityUp).normalized;

            // Calculate the desired move direction
            Vector3 desiredMoveDirection = (camUp * _direction.z + camRight * _direction.x).normalized;

            // Move the character
            _rigidbody.MovePosition(_rigidbody.position + desiredMoveDirection * _speed * Time.fixedDeltaTime);

            // Rotate the character to face the movement direction
            if (desiredMoveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection, gravityUp);
                _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.fixedDeltaTime * 10f));
            }
        }

        // Update animator
        // _animator.SetBool("isRunning", isRunning);
    }
}
