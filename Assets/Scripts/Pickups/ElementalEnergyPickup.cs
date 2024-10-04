using UnityEngine;

public class ElementalEnergyPickup : MonoBehaviour
{
    public ElementType elementType; // Type of elemental energy
    public int energyAmount = 1;    // Amount of energy provided by this pickup

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Access the player's energy manager and add energy
            PlayerEnergyManager energyManager = collision.gameObject.GetComponent<PlayerEnergyManager>();
            if (energyManager != null)
            {
                energyManager.AddEnergy(elementType, energyAmount);
                // Destroy the pickup after collection
                Destroy(gameObject);
            }
        }
    }
}
