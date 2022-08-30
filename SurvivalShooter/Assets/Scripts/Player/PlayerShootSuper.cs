using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootSuper : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    float timer;
    public int shootableMask;
    ParticleSystem gunParticles;
    AudioSource gunAudio;
    Light gunLight;
    public Light faceLight;

    float effectsDisplayTime = 0.2f;

    private void Awake()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Fire2") && timer >= timeBetweenBullets && Time.timeScale != 0)
            Shoot();
        if (timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects();
    }

    public void DisableEffects()
    {
        faceLight.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;
        gunAudio.Play();
        gunLight.enabled = true;
        faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        var allTargets = new List<EnemyIdentifier>();
        var shootResult = Physics.RaycastAll(transform.position, transform.forward, range, 1 << shootableMask);
        foreach (var res in shootResult)
        {
            EnemyIdentifier e = res.collider.GetComponent<EnemyIdentifier>();
            if (e != null)
                allTargets.Add(e);
        }

        foreach (var e in EnemyIdentifier.enemies)
        {
            var dist = (transform.position - e.transform.position).magnitude;
            if (dist < 2.0f)
            {
                bool duplicated = false;
                foreach (var res in shootResult)
                {
                    EnemyIdentifier shootResEnemy = res.collider.GetComponent<EnemyIdentifier>();
                    if (shootResEnemy == e)
                    {
                        duplicated = true;
                    }
                }

                if (!duplicated)
                {
                    allTargets.Add(e);
                }
            }
        }

        foreach (var e in allTargets)
        {
            e.health.TakeDamage(damagePerShot);
            e.move.Knockback(75, transform.position);
        }
    }
}
