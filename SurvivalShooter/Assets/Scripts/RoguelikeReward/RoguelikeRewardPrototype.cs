using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class RoguelikeRewardPrototype : ScriptableObject
{
    public string id;
    public string title;
    [Multiline]
    public string desc;
    public Sprite sp;


}
