using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : PlayerComponent
{

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    public Hemophagia hemophagia;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    public int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    public Light faceLight;

    float effectsDisplayTime = 0.2f;

    private void Awake()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            Shoot();
        if (timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects();
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
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

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, 1 << shootableMask))
        {
            //Debug.Log(shootHit.transform.gameObject);
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(damagePerShot);
                if (hemophagia != null)
                    hemophagia.SuckBlood();


            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
