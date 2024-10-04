using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public int damage = 2;
    public float knockbackForce = 200f;
    public float areaRadius = 2f;
    public LayerMask enemyLayer;

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
        // Destroy the rock after impact
        Destroy(gameObject);
    }
}
