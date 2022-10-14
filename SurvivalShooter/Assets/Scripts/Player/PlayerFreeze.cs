using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
    public float slowDown = 100;
    public float duration = 3f;

    public void Slow()
    {
        EnemyMovement.Freeze() ;
    }
}

