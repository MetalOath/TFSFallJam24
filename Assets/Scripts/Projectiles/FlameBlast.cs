using UnityEngine;

public class FlameBlast : MonoBehaviour
{
    public int damage = 2;
    public float stunDuration = 1f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                // Apply stun effect
                StartCoroutine(enemy.Stun(stunDuration));
            }
        }
        // Destroy the flame blast after impact
        Destroy(gameObject);
    }
}
