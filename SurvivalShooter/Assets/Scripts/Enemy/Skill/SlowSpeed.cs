using UnityEngine;

public class SlowSpeed : MonoBehaviour
{
    public GameObject character;
    public float slowDown = 3;
    private float speedTimestamp;
    public float slowRate = 1.5f;

    PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Slow()
    {
        playerMovement.speed  =- slowDown;
        if (Time.time - speedTimestamp > slowRate)
        {
            playerMovement.speed =+ slowDown;
        }
    }
    /*
    private void Start()
    {
        speedTimestamp = Time.time;
    }

    private void Update()
    {
        if (Time.time > speedTimestamp)
        {
            speedTimestamp += slowRate;

            Slow();
        }
    }
    */
}
