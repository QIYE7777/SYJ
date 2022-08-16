using UnityEngine;

public class SpawnEnemyBehaviour : MonoBehaviour
{
    private void Start()
    {
        var r = GetComponent<MeshRenderer>();
        r.enabled = false;

        var c = GetComponent<Collider>();
        if (c != null)
            c.enabled = false;
    }

    public void Spawn(EnemyPrototype e)
    {
        if (e == null)
            return;
        Instantiate(e.prefab, transform.position, transform.rotation);
    }
}