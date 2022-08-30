﻿using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent nav;
    EnemyIdentifier id;

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
        var player = PlayerBehaviour.instance;
        var playerHealth = player.health;
        if (id.health.hp > 0 && playerHealth.currentHealth > 0)
            nav.SetDestination(player.transform.position);
        else
            nav.enabled = false;
    }
}