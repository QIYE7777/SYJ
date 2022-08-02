using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HitSpecialEffectData hitSpecialEffect;

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    PlayerSpecialState playerSpecialState;

    bool playerInRange;
    float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerSpecialState = player.GetComponent<PlayerSpecialState>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }

        if (playerHealth.currentHealth <= 0)
            anim.SetTrigger("PlayerDead");
    }

    void Attack()
    {
        timer = 0f;

        playerHealth.TakeDamage(attackDamage);
        if (hitSpecialEffect != null && hitSpecialEffect.effectType != HitSpecialEffectData.HitSpecialEffectType.None)
            playerSpecialState.ApplyHitSpecialEffect(hitSpecialEffect);
    }
}
