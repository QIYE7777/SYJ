using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;
    public SpawnBoltOnDeath spawnBoltOnDeath;
    public SpawnPoisonCloudOnDeath spawnPoisonCloudOnDeath;
    public ExplodeOnDeath explodeOnDeath;
    EnemyIdentifier id;
    public bool canPlayWoundAnim = true;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSinking)
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead) return;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        TakeDamage(amount);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        enemyAudio.Play();
        float damageRatio = (float)amount / (float)startingHealth;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            if (damageRatio > 0.25f ||
                (damageRatio > 0.1f && Random.value > 0.6f) ||
                 (damageRatio > 0.01f && Random.value > 0.85f))
            {
                if (canPlayWoundAnim)
                    id.anim.SetTrigger("wound");
            }
        }
    }

    void Death()
    {
        if (isDead)
            return;
        isDead = true;

        if (spawnBoltOnDeath != null)
            spawnBoltOnDeath.Spawn();

        if (spawnPoisonCloudOnDeath != null)
            spawnPoisonCloudOnDeath.Spawn();

        if (explodeOnDeath != null)
            explodeOnDeath.Spawn();

        capsuleCollider.isTrigger = true;
        id.anim.SetTrigger("die");

        enemyAudio.clip = deathClip;
        enemyAudio.Play();

        Invoke("RemoveDeathBody", 0.35f);
    }

    public void StartSinking()
    {

    }

    public void RemoveDeathBody()
    {
        if (isSinking)
            return;

        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;

        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }
}
