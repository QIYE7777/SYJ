using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool isPlayerInRange;
    public float explosionEndRate = 0.01f;
    public int damage;
    private float explosionEndTimestamp;
    float thatTime;
    float nowTime;

    private void Awake()
    {
        isPlayerInRange = false;
    }

    /*private void Start()
    {
        ExplosionEndTimestamp = Time.time;
    }
    */

    private void Update()
    {
        nowTime = Time.time;
        if (isPlayerInRange)
                PlayerMovement.instance.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
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
            thatTime = Time.time;
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
            if (nowTime  > explosionEndRate + thatTime)
            {
                isPlayerInRange = false;
            }
        }
    }
}
