using UnityEngine;

public class UpgradeSpeed : UpgradeGeneral
{
    public UpgradeSpeed()
    {
        upgradeName = "+5 Velocidad";
        description = "Te mueves más rápido";
    }

    public override void Apply()
    {
        PlayerMovement playerMovement = Object.FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.IncreaseSpeed(5f);
            Debug.Log("Upgrade aplicado: " + upgradeName);
        }
    }

}
