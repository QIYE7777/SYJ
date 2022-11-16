using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : PlayerComponent
{
    public int hpMax = 100;
    public int currentHealth;

    public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    int preventDeathCount = 1;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = hpMax;
    }

    public void Start()
    {
        currentHealth = hpMax - CombatManager.instance.lostHealth;
        if (currentHealth <= 0)
            currentHealth = 1;

        RefreshHpBar();
        preventDeathCount = 1;
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        if (currentHealth > hpMax)
            currentHealth = hpMax;

        RefreshHpBar();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        HudBehaviour.instance.OnDamaged();
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            if (preventDeathCount > 0 && currentHealth > -20)
            {
                currentHealth = 1;
                preventDeathCount--;
            }
        }

        if (currentHealth < 0)
            currentHealth = 0;

        RefreshHpBar();
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
            Death();
    }

    void RefreshHpBar()
    {
        float currentHpRatio = (float)currentHealth / (float)hpMax;
        HudBehaviour.instance.SetHpBar(Mathf.Pow(currentHpRatio, 1.5f));//让最后的血条显得更耐用的一点，增加玩家残血过关的概率
    }

    void Death()
    {
        isDead = true;
        playerShooting.DisableEffects();
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneSwitcher.instance.RestartCurrentLevel();
    }

    public void SaveHp()
    {
        CombatManager.instance.lostHealth = hpMax - currentHealth;
    }
}
