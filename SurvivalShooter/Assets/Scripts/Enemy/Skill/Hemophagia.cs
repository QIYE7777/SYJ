using UnityEngine;
using RoguelikeCombat;

public class Hemophagia : MonoBehaviour
{
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void LifeSteal()
    {
        int healPerShoot = 0;

        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_80))
        {
            healPerShoot = 5;
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_10))
        {
            healPerShoot = 2;
        }
        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_5))
        {
            healPerShoot = 1;
        }


        if (healPerShoot > 0)
        {
            playerHealth.Heal(healPerShoot);
            Debug.Log(healPerShoot);
        }
    }
}