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
        var r = exit.transform.GetComponent<MeshRenderer>();
        r.enabled = false;

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

    void TrySpawn()
    {
        Debug.Log("生成第" + (_nextWaveIndex + 1) + "波");
        List<EnemyPrototype> normalEnemies = SceneSwitcher.instance.roomPrototype.normalEnemies;
        List<EnemyPrototype> specialEnemies = SceneSwitcher.instance.roomPrototype.specialEnemies;
        List<EnemyPrototype> verySpecialEnemies = SceneSwitcher.instance.roomPrototype.verySpecialEnemies;
        for (int i = 0; i < normalSpawns.Count; i++)
        {
            normalSpawns[i].Spawn(normalEnemies[i]);
        }
        for (int i = 0; i < specialSpawns.Count; i++)
        {
            specialSpawns[i].Spawn(specialEnemies[i]);
        }
        for (int i = 0; i < verySpecialSpawns.Count; i++)
        {
            verySpecialSpawns[i].Spawn(verySpecialEnemies[i]);
        }

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
        var player = Instantiate(SceneSwitcher.instance.playerPrefab, entrance.position, entrance.rotation, entrance.transform.parent);
        CameraFollow.instance.Init(player.transform);
    }
}