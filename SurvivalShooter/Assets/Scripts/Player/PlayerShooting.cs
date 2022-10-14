using RoguelikeCombat;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : PlayerComponent
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    public Hemophagia hemophagia { get; private set; }
    public PlayerFreeze freeze;

    float timer;
    RaycastHit shootHit;
    public int shootableMask;
    ParticleSystem gunParticles;
    public LineRenderer gunLinePrefab;
    AudioSource gunAudio;
    Light gunLight;
    public Light faceLight;

    float effectsDisplayTime = 0.2f;

    private void Start()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        hemophagia = host.GetComponent<Hemophagia>();
        freeze = GetComponent<PlayerFreeze>();
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

        FireShoot(0);
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.MultiShoot_add2shoot_30degree))
        {
            //add 2 shoot
            FireShoot(35);
            FireShoot(-35);
        }
    }


    void FireShoot(float angleOffset = 0)
    {
        var gunLine = Instantiate(gunLinePrefab, gunLinePrefab.transform.parent);
        gunLine.gameObject.SetActive(true);
        gunLine.transform.localPosition = Vector3.zero;
        gunLine.SetPosition(0, Vector3.zero);
        Destroy(gunLine.gameObject, timeBetweenBullets);

        Ray shootRay = new Ray();
        shootRay.origin = transform.position;
        Vector3 relativeDirection = Vector3.forward;

        if (angleOffset == 0)
        {
            shootRay.direction = transform.forward;
        }
        else if (angleOffset > 0)
        {
            shootRay.direction = Vector3.RotateTowards(transform.forward, transform.right, angleOffset * Mathf.Deg2Rad, float.MaxValue);
            relativeDirection = Vector3.RotateTowards(Vector3.forward, Vector3.right, angleOffset * Mathf.Deg2Rad, float.MaxValue);
        }
        else if (angleOffset < 0)
        {
            shootRay.direction = Vector3.RotateTowards(transform.forward, -transform.right, -angleOffset * Mathf.Deg2Rad, float.MaxValue);
            relativeDirection = Vector3.RotateTowards(Vector3.forward, -Vector3.right, -angleOffset * Mathf.Deg2Rad, float.MaxValue);
        }

        if (Physics.Raycast(shootRay, out shootHit, range, 1 << shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(damagePerShot);
            hemophagia.SuckBlood();

            gunLine.SetPosition(1, relativeDirection * (shootHit.point - shootRay.origin).magnitude);

            freeze.Slow();

        }
        else
        {
            gunLine.SetPosition(1, relativeDirection * range);
        }
    }
}
