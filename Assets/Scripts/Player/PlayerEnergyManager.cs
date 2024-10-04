using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyManager : MonoBehaviour
{
    public Dictionary<ElementType, int> elementalEnergies = new Dictionary<ElementType, int>();
    public ElementType currentAffinity = ElementType.Null;

    public Text energyUIText; // Assign this in the Inspector
    public Image affinityIcon; // UI image to display current affinity

    void Start()
    {
        // Initialize energies
        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
        {
            elementalEnergies[element] = 0;
        }
        UpdateEnergyUI();
    }

    public void AddEnergy(ElementType elementType, int amount)
    {
        elementalEnergies[elementType] += amount;
        DetermineHighestAffinity();
        UpdateEnergyUI();
    }

    void DetermineHighestAffinity()
    {
        // Find the element with the highest energy
        int maxEnergy = 0;
        foreach (KeyValuePair<ElementType, int> kvp in elementalEnergies)
        {
            if (kvp.Value > maxEnergy)
            {
                maxEnergy = kvp.Value;
                currentAffinity = kvp.Key;
            }
        }
        UpdateAffinityUI();
    }

    public void CycleElementalAffinity()
    {
        // Get elements with collected energy
        List<ElementType> availableElements = new List<ElementType>();
        foreach (KeyValuePair<ElementType, int> kvp in elementalEnergies)
        {
            if (kvp.Value > 0)
            {
                availableElements.Add(kvp.Key);
            }
        }

        if (availableElements.Count == 0)
        {
            currentAffinity = ElementType.Null;
            UpdateAffinityUI();
            return;
        }

        int currentIndex = availableElements.IndexOf(currentAffinity);
        int nextIndex = (currentIndex + 1) % availableElements.Count;
        currentAffinity = availableElements[nextIndex];
        UpdateAffinityUI();
    }

    public void UpdateEnergyUI()
    {
        if (energyUIText != null)
        {
            energyUIText.text = "Energies:\n";
            foreach (KeyValuePair<ElementType, int> kvp in elementalEnergies)
            {
                energyUIText.text += $"{kvp.Key}: {kvp.Value}\n";
            }
        }
    }

    void UpdateAffinityUI()
    {
        // Update UI elements to reflect current affinity
        if (affinityIcon != null)
        {
            // Change the color of the affinity icon based on the current affinity
            switch (currentAffinity)
            {
                case ElementType.Null:
                    affinityIcon.color = Color.gray;
                    break;
                case ElementType.Earth:
                    affinityIcon.color = Color.green;
                    break;
                case ElementType.Water:
                    affinityIcon.color = Color.blue;
                    break;
                case ElementType.Fire:
                    affinityIcon.color = Color.red;
                    break;
                default:
                    affinityIcon.color = Color.white;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Affinity Icon is not assigned in the Inspector.");
        }
    }
}
