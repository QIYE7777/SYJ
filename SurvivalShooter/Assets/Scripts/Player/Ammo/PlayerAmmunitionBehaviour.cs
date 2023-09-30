using UnityEngine;
using System.Collections.Generic;
using RoguelikeCombat;

public class PlayerAmmunitionBehaviour : MonoBehaviour
{
    public static PlayerAmmunitionBehaviour instance;

    public int maxAmmo = 6;
    public AmmoUiBehaviour ammoPrefab;
    public RectTransform ammoParent;

    public float reloadTime = 2;

    float _reloadDoneTimestamp;//这个数值小于等于0 表示不在换弹，否则表示换弹结束的时间点

    [HideInInspector]
    public int currentAmmoCount;
    [HideInInspector]
    public List<AmmoUiBehaviour> ammos;

    private void Awake()
    {
        instance = this;
        _reloadDoneTimestamp = 0;
        currentAmmoCount = maxAmmo;
    }
    
    private void Start()
    {
        BuildAmmoUi();

    }

    void BuildAmmoUi()
    {
        ammos = new List<AmmoUiBehaviour>();
        for (var i = 0; i < maxAmmo; i++)
        {
            var newAmmo = Instantiate(ammoPrefab, ammoParent);
            newAmmo.SetToFilled();
            newAmmo.reloadAnimTime = reloadTime;
            ammos.Add(newAmmo);
        }
    }

    void ClearAmmoUi()
    {
        Destroy(ammoPrefab);
    }



    public void MoreBullet()
    {
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_5))
        {
            maxAmmo = 5;
            ClearAmmoUi();
            BuildAmmoUi();
        }
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_10))
        {
            maxAmmo = 10;
            ClearAmmoUi();
            BuildAmmoUi();
        }
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_00))
        {
            maxAmmo = 1;
            reloadTime = 0;
            ClearAmmoUi();
            BuildAmmoUi();
        }
    }

    public bool IsReloading()
    {
        if (_reloadDoneTimestamp <= 0)
        {
            return false;
        }

        if (Time.time > _reloadDoneTimestamp)
        {
            return false;
        }

        return true;
    }

    public void OnFired()
    {

        currentAmmoCount -= 1;
        for (var i = 0; i < maxAmmo; i++)
        {
            if (i + 1 <= currentAmmoCount)
            {
                ammos[i].SetToFilled();
            }
            else
            {
                ammos[i].SetToEmpty();
            }
        }

        if (currentAmmoCount <= 0)
        {
            _reloadDoneTimestamp = Time.time + reloadTime;
            currentAmmoCount = maxAmmo;

            for (var i = 0; i < maxAmmo; i++)
            {
                ammos[i].ReloadAnim();
            }
        }
    }
}
