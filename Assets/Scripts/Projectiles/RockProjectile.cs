using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public int damage = 2;
    public float knockbackForce = 200f;
    public float areaRadius = 2f;
    public LayerMask enemyLayer;
    public GameObject impactEffect;

    private Rigidbody _rigidbody;
    private float _timer = 0f;
    public float lifetime = 5f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * 20f; // Adjust speed as needed
        _rigidbody.useGravity = false; // Set to true if you want gravity to affect the rock
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detect enemies within area of effect
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaRadius, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                // Apply knockback
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDir * knockbackForce);
            }
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Destroy the rock after impact
        Destroy(gameObject);
    }
}
