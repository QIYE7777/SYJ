using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent nav;
    EnemyIdentifier id;
    public float dec = 100;
    float _knockSpeed;
    Vector3 _knockDir;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
        nav = GetComponent<NavMeshAgent>();
    }

    public void SetSpeed(float s)
    {
        nav.speed = s;
    }

    private void Start()
    {
        id.anim.SetBool("walk", true);
    }

    void Update()
    {
        if (_knockSpeed > 0)
        {
            CheckKnockback();
        }
        else
        {
            Walk();
        }
    }

    void CheckKnockback()
    {
        _knockSpeed -= Time.deltaTime * dec;
        if (_knockSpeed <= 0)
        {
            //switch to walk
            nav.enabled = true;
            return;
        }

        transform.position += _knockDir * _knockSpeed * Time.deltaTime;
    }

    void Walk()
    {
        var player = PlayerBehaviour.instance;
        var playerHealth = player.health;
        if (id.health.hp > 0 && playerHealth.currentHealth > 0)
            nav.SetDestination(player.transform.position);
        else
            nav.enabled = false;
    }

    public void Knockback(float speed, Vector3 origin)
    {
        var dir = transform.position - origin;
        dir.y = 0;
        _knockDir = dir.normalized;
        _knockSpeed = speed;
        nav.enabled = false;
    }
}