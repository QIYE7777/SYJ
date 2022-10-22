using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    bool isPlayerInRange;
    public float poisonAttackRate = 0.2f;
    public int damage;
    private float poisonAttackTimestamp;

    private void Awake()
    {
        isPlayerInRange = false;
    }

    private void Start()
    {
        poisonAttackTimestamp = com.GameTime.time;
    }

    private void Update()
    {
        if (com.GameTime.time > poisonAttackTimestamp)
        {
            poisonAttackTimestamp += poisonAttackRate;
            if (isPlayerInRange)
                PlayerMovement.instance.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<BoltBehaviour>())
            return;
        if (other.transform.GetComponent<EnemyMovement>())
            return;

        if (other.transform == PlayerMovement.instance.transform)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<BoltBehaviour>())
            return;
        if (other.transform.GetComponent<EnemyMovement>())
            return;

        if (other.transform == PlayerMovement.instance.transform)
        {
            isPlayerInRange = false;
        }
    }
}
