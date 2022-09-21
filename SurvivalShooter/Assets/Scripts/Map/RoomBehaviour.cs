using UnityEngine;
using System.Collections.Generic;

public class RoomBehaviour : MonoBehaviour
{
    public List<SpawnEnemyBehaviour> normalSpawns;
    public List<SpawnEnemyBehaviour> specialSpawns;
    public List<SpawnEnemyBehaviour> verySpecialSpawns;

    public DoorBehaviour exit;
    public Transform entrance;

    float _nextWaveTimestamp;
    int _nextWaveIndex;
    public static RoomBehaviour instance;

    private void Awake()
    {
        instance = this;
    }

    void HideExitEndEntranceCube()
    {
        if (exit != null)
        {
            var r = exit.transform.GetComponent<MeshRenderer>();
            r.enabled = false;
        }

        var r1 = entrance.GetComponent<MeshRenderer>();
        r1.enabled = false;

        var c1 = entrance.GetComponent<Collider>();
        if (c1 != null)
            c1.enabled = false;
    }

    private void Start()
    {
        HideExitEndEntranceCube();
        SpawnPlayer();

        _nextWaveIndex = 0;
        _nextWaveTimestamp = Time.time + SceneSwitcher.instance.roomPrototype.spawnWaves[_nextWaveIndex];
    }

    private void Update()
    {
        if (_nextWaveTimestamp >= 0 && Time.time > _nextWaveTimestamp)
        {
            TrySpawn();
        }
    }

    void SpawnOneKindOfEnemy(List<EnemyPrototype> enemies, List<SpawnEnemyBehaviour> spots)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            if (spots == null)
                continue;
            if (i >= enemies.Count)
                continue;
            spots[i].Spawn(enemies[i]);
        }
    }

    void TrySpawn()
    {
        Debug.Log("生成第" + (_nextWaveIndex + 1) + "波");
        List<EnemyPrototype> normalEnemies = SceneSwitcher.instance.roomPrototype.normalEnemies;
        List<EnemyPrototype> specialEnemies = SceneSwitcher.instance.roomPrototype.specialEnemies;
        List<EnemyPrototype> verySpecialEnemies = SceneSwitcher.instance.roomPrototype.verySpecialEnemies;
        SpawnOneKindOfEnemy(normalEnemies, normalSpawns);
        SpawnOneKindOfEnemy(specialEnemies, specialSpawns);
        SpawnOneKindOfEnemy(verySpecialEnemies, verySpecialSpawns);

        _nextWaveIndex += 1;
        if (IsSpawnDone())
        {
            Debug.Log("生成完了");
            _nextWaveTimestamp = -1;
        }
        else
        {
            _nextWaveTimestamp = Time.time + SceneSwitcher.instance.roomPrototype.spawnWaves[_nextWaveIndex];
        }
    }

    public bool IsSpawnDone()
    {
        return _nextWaveIndex >= SceneSwitcher.instance.roomPrototype.spawnWaves.Count;
    }

    void SpawnPlayer()
    {
        var spawnPosition = entrance.position;
        spawnPosition.y = 0;
        var player = Instantiate(SceneSwitcher.instance.playerPrefab, spawnPosition, entrance.rotation, entrance.transform.parent);
        CameraFollow.instance.Init(player.transform);
    }
}