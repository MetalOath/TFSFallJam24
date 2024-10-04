using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public int damage = 1;                // Damage dealt to an enemy
    public float speed = 25f;             // Movement speed of the projectile
    public float lifetime = 5f;           // Time before the projectile is destroyed
    public GameObject impactEffect;       // Optional visual effect upon impact

    private Rigidbody _rigidbody;
    private float _timer = 0f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // Set the projectile's velocity in the forward direction
        _rigidbody.velocity = transform.forward * speed;
        // Ensure gravity is not affecting the projectile
        _rigidbody.useGravity = false;
    }

    void Update()
    {
        // Destroy the projectile after its lifetime expires
        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Instantiate impact effect if assigned
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Handle collision with the environment
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
        else
        {
            // Optionally, handle collisions with other objects
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
