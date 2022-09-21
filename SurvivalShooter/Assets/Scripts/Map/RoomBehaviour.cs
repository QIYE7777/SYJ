using UnityEngine;
using System.Collections.Generic;

public class RoomBehaviour : MonoBehaviour
{
    public List<SpawnEnemyBehaviour> normalSpawns;
    public List<SpawnEnemyBehaviour> specialSpawns;
    public List<SpawnEnemyBehaviour> verySpecialSpawns;

    public DoorBehaviour exit;
    public Transform entrance;

    float _nextWaveWaitTime;
    int _waveIndex;
    public static RoomBehaviour instance;
    bool _hasWaveToSpawn;

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

        _hasWaveToSpawn = false;
        _waveIndex = 0;
        _nextWaveWaitTime = 0;
    }

    private void Update()
    {
        if (CombatManager.instance.HasEnemyLeft(true))
            return;//有敌人时，什么都不做

        _nextWaveWaitTime -= Time.deltaTime;
        if (_nextWaveWaitTime < 0)
        {
            if (_hasWaveToSpawn)
                TrySpawn();
            else
                PrepareWave();
        }
    }

    void PrepareWave()
    {
        Debug.Log("PrepareWave");
        if (IsSpawnDone())
            return;
        Debug.Log("准备生成第" + (_waveIndex + 1) + "波");
        _nextWaveWaitTime = SceneSwitcher.instance.roomPrototype.spawnWaves[_waveIndex];
        _hasWaveToSpawn = true;
    }

    void TrySpawn()
    {
        Debug.Log("生成第" + (_waveIndex + 1) + "波");
        if (IsSpawnDone())
        {
            Debug.Log("生成完了");
            return;
        }

        List<EnemyPrototype> normalEnemies = SceneSwitcher.instance.roomPrototype.normalEnemies;
        List<EnemyPrototype> specialEnemies = SceneSwitcher.instance.roomPrototype.specialEnemies;
        List<EnemyPrototype> verySpecialEnemies = SceneSwitcher.instance.roomPrototype.verySpecialEnemies;
        SpawnOneKindOfEnemy(normalEnemies, normalSpawns);
        SpawnOneKindOfEnemy(specialEnemies, specialSpawns);
        SpawnOneKindOfEnemy(verySpecialEnemies, verySpecialSpawns);
        _waveIndex += 1;
        _hasWaveToSpawn = false;
    }

    void SpawnOneKindOfEnemy(List<EnemyPrototype> enemies, List<SpawnEnemyBehaviour> spots)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            if (spots == null)
                continue;
            if (i >= enemies.Count)
                continue;
            spots[i].Spawn(enemies[i], Random.Range(0f,2.5f));
        }
    }

    public bool IsSpawnDone()
    {
        return _waveIndex >= SceneSwitcher.instance.roomPrototype.spawnWaves.Count;
    }

    void SpawnPlayer()
    {
        var spawnPosition = entrance.position;
        spawnPosition.y = 0;
        var player = Instantiate(SceneSwitcher.instance.playerPrefab, spawnPosition, entrance.rotation, entrance.transform.parent);
        CameraFollow.instance.Init(player.transform);
    }
}