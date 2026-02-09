using UnityEngine;

public class UpgradeHealthMedium : UpgradeGeneral
{
    public UpgradeHealthMedium()
    {
        upgradeName = "+10 Vida";
        description = "Aumenta la vida máxima en 10";
    }

    public override void Apply()
    {
        PlayerHealth playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(10);
            Debug.Log("Upgrade aplicado: " + upgradeName);
        }
    }
}
