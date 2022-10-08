using UnityEngine;

public class Hemophagia : MonoBehaviour
{
    PlayerHealth playerHealth;
    public int healPerShoot = 0;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void SuckBlood()
    {
        if (healPerShoot>0)
        {
            playerHealth.Heal(healPerShoot);
        }
    }
}