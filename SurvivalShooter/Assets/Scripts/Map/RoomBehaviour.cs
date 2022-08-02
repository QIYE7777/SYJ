using UnityEngine;
using System.Collections.Generic;

public class RoomBehaviour : MonoBehaviour
{
    public List<SpawnEnemyBehaviour> normalSpawns;
    public List<SpawnEnemyBehaviour> specialSpawns;
    public List<SpawnEnemyBehaviour> verySpecialSpawns;

    public DoorBehaviour exit;
    public Transform entrance;

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        var player = Instantiate(SceneSwitcher.instance.playerPrefab, entrance.position, entrance.rotation, entrance.transform.parent);
        CameraFollow.instance.Init(player.transform);
    }
}