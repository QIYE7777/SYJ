using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class RoguelikeRewardPrototype : ScriptableObject
{
    public RoguelikeUpgradeId id;
    public string title;
    [Multiline]
    public string desc;
    public Sprite sp;

    public RoguelikeUpgradeId dependency;

}

//枚举类型
public enum RoguelikeUpgradeId
{
    None = 0,
    Leech_5 = 1,
    Leech_10 = 2,
  
    MultiShoot_add2shoot_30degree = 3,

    SlowDown = 10,
}