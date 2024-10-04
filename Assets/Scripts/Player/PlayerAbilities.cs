using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerEnergyManager energyManager;
    private PlayerController playerController;

    // Ability parameters
    public int nullEnergyCostPerTick = 1;
    public int earthEnergyCost = 5;
    public int waterEnergyCost = 3;
    public int fireEnergyCost = 4;

    public float rockThrowCooldown = 2f;
    public float swiftDashCooldown = 1f;
    public float flameBlastCooldown = 1.5f;

    private float rockThrowTimer = 0f;
    private float swiftDashTimer = 0f;
    private float flameBlastTimer = 0f;

    private bool deathTouchShieldActive = false;

    public GameObject basicProjectilePrefab;  // Assign in the Inspector
    public float basicAttackCooldown = 0.5f;  // Time between basic attacks

    private float basicAttackTimer = 0f;      // Timer to track cooldown

    // Reference to ability prefabs
    public GameObject rockPrefab;
    public GameObject flameBlastPrefab;

    void Start()
    {
        energyManager = GetComponent<PlayerEnergyManager>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Update cooldown timers
        if (rockThrowTimer > 0)
            rockThrowTimer -= Time.deltaTime;
        if (swiftDashTimer > 0)
            swiftDashTimer -= Time.deltaTime;
        if (flameBlastTimer > 0)
            flameBlastTimer -= Time.deltaTime;
        if (basicAttackTimer > 0)
            basicAttackTimer -= Time.deltaTime;

        // Left-click to activate ability
        if (Input.GetMouseButtonDown(0))
        {
            ActivatePrimaryAttack();
        }

        // Death Touch Shield energy consumption
        if (deathTouchShieldActive)
        {
            // Consume energy over time
            if (Time.time % 0.5f < Time.deltaTime)
            {
                if (energyManager.elementalEnergies[ElementType.Null] >= nullEnergyCostPerTick)
                {
                    energyManager.elementalEnergies[ElementType.Null] -= nullEnergyCostPerTick;
                    energyManager.UpdateEnergyUI();
                    // Damage nearby enemies
                    DamageNearbyEnemies();
                }
                else
                {
                    // Not enough energy, deactivate shield
                    ToggleDeathTouchShield(false);
                }
            }
        }
    }

    void ActivatePrimaryAttack()
    {
        switch (energyManager.currentAffinity)
        {
            case ElementType.Null:
                ToggleDeathTouchShield(!deathTouchShieldActive);
                break;
            case ElementType.Earth:
                PerformRockThrow();
                break;
            case ElementType.Water:
                ExecuteSwiftDash();
                break;
            case ElementType.Fire:
                FireFlameBlast();
                break;
            default:
                PerformBasicAttack();
                break;
        }
    }

    // Implement the abilities below...

    void ToggleDeathTouchShield(bool activate)
    {
        deathTouchShieldActive = activate;
        // Activate or deactivate shield visuals
        // Example: shieldVisual.SetActive(activate);
    }

    void DamageNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemy = hitCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }

    void PerformRockThrow()
    {
        if (rockThrowTimer > 0) return;
        if (energyManager.elementalEnergies[ElementType.Earth] >= earthEnergyCost)
        {
            energyManager.elementalEnergies[ElementType.Earth] -= earthEnergyCost;
            energyManager.UpdateEnergyUI();

            // Instantiate the rock projectile
            Vector3 spawnPosition = transform.position + transform.forward;
            GameObject rock = Instantiate(rockPrefab, spawnPosition, transform.rotation);

            rockThrowTimer = rockThrowCooldown;
        }
    }


    void ExecuteSwiftDash()
    {
        if (swiftDashTimer > 0) return;
        if (energyManager.elementalEnergies[ElementType.Water] >= waterEnergyCost)
        {
            energyManager.elementalEnergies[ElementType.Water] -= waterEnergyCost;
            energyManager.UpdateEnergyUI();

            // Start the dash coroutine
            StartCoroutine(SwiftDashCoroutine());

            swiftDashTimer = swiftDashCooldown;
        }
    }

    IEnumerator SwiftDashCoroutine()
    {
        float dashDuration = 0.2f; // Duration of the dash
        float dashSpeedMultiplier = 4f; // Speed multiplier during dash

        Rigidbody playerRb = playerController.GetComponent<Rigidbody>();
        Vector3 dashDirection = playerController.GetMovementDirection().normalized;

        // Store original drag to reset later
        float originalDrag = playerRb.drag;

        // Remove drag to allow smooth movement
        playerRb.drag = 0f;

        // Apply dash force
        playerRb.velocity = dashDirection * playerController.Speed * dashSpeedMultiplier;

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;

            // Handle collision with enemies
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    EnemyController enemy = hitCollider.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(2);
                        // Knockback enemy
                        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                        Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDir * 200f);
                    }
                }
            }

            yield return null;
        }

        // Reset player velocity and drag
        playerRb.velocity = Vector3.zero;
        playerRb.drag = originalDrag;
    }

    void FireFlameBlast()
    {
        if (flameBlastTimer > 0) return;
        if (energyManager.elementalEnergies[ElementType.Fire] >= fireEnergyCost)
        {
            energyManager.elementalEnergies[ElementType.Fire] -= fireEnergyCost;
            energyManager.UpdateEnergyUI();

            // Instantiate and fire the flame blast
            GameObject flame = Instantiate(flameBlastPrefab, transform.position + transform.forward, transform.rotation);
            Rigidbody rb = flame.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 25f; // Adjust speed as needed

            flameBlastTimer = flameBlastCooldown;
        }
    }

    void PerformBasicAttack()
    {
        if (basicAttackTimer > 0) return;  // Check if attack is on cooldown

        // Instantiate the projectile slightly in front of the player to avoid collision with self
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f;
        GameObject projectile = Instantiate(basicProjectilePrefab, spawnPosition, transform.rotation);

        // Reset the cooldown timer
        basicAttackTimer = basicAttackCooldown;

        // Optionally play a firing sound
        // AudioSource.PlayClipAtPoint(basicAttackSound, transform.position);
    }
}
