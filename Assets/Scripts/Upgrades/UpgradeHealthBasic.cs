using UnityEngine;

public class UpgradeHealthBasic : UpgradeGeneral
{
    public UpgradeHealthBasic()
    {
        upgradeName = "+5 Vida";
        description = "Aumenta la vida máxima en 5";
    }

    public override void Apply()
    {
        // Usar FindObjectOfType en lugar de FindAnyObjectByType para mayor compatibilidad
        PlayerHealth playerHealth = Object.FindAnyObjectByType  <PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(5);
            Debug.Log($"Upgrade aplicado: {upgradeName}. Nueva vida: {playerHealth.maxHealth}");
        }
        else
        {
            Debug.LogError("No se encontró PlayerHealth en la escena!");
        }
    }

}
