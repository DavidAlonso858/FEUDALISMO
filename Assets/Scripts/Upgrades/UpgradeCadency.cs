using UnityEngine;

public class UpgradeCadency : UpgradeGeneral
{
    public UpgradeCadency()
    {
        upgradeName = "Cadencia +";
        description = "Disparas más rápido";
    }

    public override void Apply()
    {
        PlayerShooting playerShooting = Object.FindAnyObjectByType<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.IncreaseFireRate(0.02f);
            Debug.Log("Upgrade aplicado: " + upgradeName);
        }
    }
}
