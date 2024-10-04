using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectGravity))]
public class EnemyController : MonoBehaviour
{
    private Transform _player;           // Reference to the player's transform
    private Rigidbody _rigidbody;        // Rigidbody component for physics
    private ObjectGravity _gravityBody;  // Reference to ObjectGravity component
    private float _speed = 5f;           // Movement speed towards the player
    public int maxHealth = 3;
    private int currentHealth;

    private bool isStunned = false;

    // Prefabs for energy pickups
    public GameObject nullPickupPrefab;
    public GameObject earthPickupPrefab;
    public GameObject waterPickupPrefab;
    public GameObject firePickupPrefab;

    void Start()
    {
        // Find the player GameObject by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found. Please ensure the player has the tag 'Player'.");
        }

        // Get components
        _rigidbody = GetComponent<Rigidbody>();
        _gravityBody = GetComponent<ObjectGravity>();

        currentHealth = maxHealth;
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Randomly select an elemental type to drop
        ElementType elementType = (ElementType)Random.Range(0, System.Enum.GetValues(typeof(ElementType)).Length);
        // Instantiate the corresponding pickup prefab
        GameObject pickupPrefab = GetPickupPrefabForElement(elementType);
        Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        // Destroy the enemy
        Destroy(gameObject);
    }

    GameObject GetPickupPrefabForElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Null:
                return nullPickupPrefab;
            case ElementType.Earth:
                return earthPickupPrefab;
            case ElementType.Water:
                return waterPickupPrefab;
            case ElementType.Fire:
                return firePickupPrefab;
            default:
                return null;
        }
    }

    public IEnumerator Stun(float duration)
    {
        isStunned = true;
        // Implement visual effect for stun if desired
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    void FixedUpdate()
    {
        if (isStunned || _player == null) return;

        // Calculate direction to the player
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;

        // Get the gravity direction
        Vector3 gravityDirection = _gravityBody.GravityDirection.normalized;

        // Project the direction onto the surface (plane perpendicular to gravity)
        Vector3 movementDirection = Vector3.ProjectOnPlane(directionToPlayer, gravityDirection).normalized;

        // Move the enemy towards the player along the surface
        _rigidbody.MovePosition(_rigidbody.position + movementDirection * _speed * Time.fixedDeltaTime);

        // Rotate the enemy to face the player along the surface
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, -gravityDirection);
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.fixedDeltaTime * 5f));
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the enemy if it collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
