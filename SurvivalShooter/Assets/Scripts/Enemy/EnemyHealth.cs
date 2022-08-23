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
    EnemyIdentifier id;

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

        enemyAudio.Play();

        currentHealth -= amount;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
            Death();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        enemyAudio.Play();
        currentHealth -= amount;

        if (currentHealth <= 0)
            Death();
    }

    void Death()
    {
        isDead = true;

        if (spawnBoltOnDeath != null)
            spawnBoltOnDeath.Spawn();

        if (spawnPoisonCloudOnDeath != null)
            spawnPoisonCloudOnDeath.Spawn();
        
        capsuleCollider.isTrigger = true;
        id.anim.SetTrigger("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }

    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;

        ScoreManager.score += scoreValue;

        Destroy(gameObject, 2f);
    }
}
