using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HitSpecialEffectData hitSpecialEffect;
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    bool playerInRange;
    float timer;

    EnemyIdentifier id;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerBehaviour.instance.gameObject)
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerBehaviour.instance.gameObject)
            playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && id.health.currentHealth > 0)
        {
            Attack();
        }

        var player = PlayerBehaviour.instance;
        if (player.health.currentHealth <= 0)
            id.anim.SetTrigger("PlayerDead");
    }

    void Attack()
    {
        timer = 0f;
        var player = PlayerBehaviour.instance;
        player.health.TakeDamage(attackDamage);
        if (hitSpecialEffect != null && hitSpecialEffect.effectType != HitSpecialEffectData.HitSpecialEffectType.None)
            player.specialState.ApplyHitSpecialEffect(hitSpecialEffect);
    }
}