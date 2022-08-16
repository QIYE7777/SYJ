using UnityEngine;
using System.Collections.Generic;

public class EnemyIdentifier : MonoBehaviour
{
    public static List<EnemyIdentifier> enemies = new List<EnemyIdentifier>();

    private void OnEnable()
    {
        enemies.Add(this);
    }

    private void OnDestroy()
    {
        enemies.Remove(this);
    }

    public static bool NoEnemyExist()
    {
        Debug.Log(enemies.Count);
        return enemies.Count == 0;
    }
}
