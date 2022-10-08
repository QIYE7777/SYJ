using UnityEngine;

public class Hemophagia : MonoBehaviour
{
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void SuckBlood()
    {
        playerHealth.Hemophagia();
    }
}
