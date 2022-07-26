using UnityEngine;

public class PlayerBlink : MonoBehaviour
{
    public float timeBetweenBlinks = 1.2f;
    float timer;

    public float blinkDistance = 3f;

    Ray blinkRay = new Ray();

    public Transform gunPos;
    public int blinkableMask;

    PlayerMovement _movement;

    public bool passThoughEnemies;
    public int damageToPassThoughEnemy = 500;
    public float checkDistanceToPassThoughEnemy = 1f;

    public GameObject trait;
    public float trailDuration;
    private float _blinkTrailDisappearTimestamp;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Jump") && timer >= timeBetweenBlinks && Time.timeScale != 0)
            Blink();

        if (trait.activeSelf && Time.time > _blinkTrailDisappearTimestamp)
        {
            trait.SetActive(false);
        }
    }

    void Blink()
    {
        trait.SetActive(true);
        _blinkTrailDisappearTimestamp = Time.time + trailDuration;

        timer = 0;
        var pos = GetBlinkTargetPlace();
        pos.y = 0;
        transform.position = pos;
    }

    Vector3 GetBlinkDirection()
    {
        //return _movement.transform.forward; 
        return _movement.movement.normalized;
    }

    Vector3 GetBlinkTargetPlace()
    {
        blinkRay.origin = gunPos.position;
        //blinkRay.direction = _movement.playerToMouse.normalized;
        blinkRay.direction = GetBlinkDirection();

        //public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask);
        RaycastHit[] blinkHits = Physics.RaycastAll(blinkRay, blinkDistance, 1 << blinkableMask);
        bool isRayBlockedByObstacle = false;

        var targetPlace = blinkRay.origin + blinkRay.direction * blinkDistance;

        foreach (var blinkHit in blinkHits)
        {
            EnemyHealth eh = blinkHit.collider.gameObject.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                //ray hits an enemy
                if (!isRayBlockedByObstacle)
                {
                    //old way to test enemies pass though
                    //not accuracy
                }
            }
            else
            {
                //ray hits an obstacle
                if (!isRayBlockedByObstacle)
                {
                    isRayBlockedByObstacle = true;
                    targetPlace = blinkHit.point;
                }
            }
        }

        if (damageToPassThoughEnemy > 0)
        {
            foreach (var e in EnemyIdentifier.enemies)
            {
                if (MathGame.NearestDistanceFromLine(blinkRay.origin, targetPlace, e.transform.position) < checkDistanceToPassThoughEnemy)
                {
                    var eh = e.GetComponent<EnemyHealth>();
                    eh.TakeDamage(damageToPassThoughEnemy);
                }
            }
        }

        return targetPlace;
    }
}