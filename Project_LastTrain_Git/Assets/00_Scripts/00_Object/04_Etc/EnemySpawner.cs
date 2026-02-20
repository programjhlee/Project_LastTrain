using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _trainBack;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] EnemyData _baseEnemyData;
    Renderer _rend;


    List<Enemy> enemies = new List<Enemy>();
    List<Dictionary<string, object>> EnemyDataTable;

    float curTime = 0;
    float _spawnTime;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        EnemyDataTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyLevelData);
        _rend = _trainBack.GetComponent<Renderer>();
        SetEnemiesData();
        GameObject enemyPool = new GameObject("EnemyPool");
        for (int i = 0; i < 50; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.name = $"Enemy_{i + 1}";
            enemy.transform.SetParent(enemyPool.transform);
            enemies.Add(enemy.GetComponent<Enemy>());
            enemy.SetActive(false);
            GravityManager.Instance.AddGravityObj(enemy.GetComponent<IGravityAffected>());
        }
        GameManager.Instance.OnStageClear += AllEnemyUnActive;
        GameManager.Instance.OnGameStart += StartEnemySpawn;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= AllEnemyUnActive;
        GameManager.Instance.OnGameStart -= StartEnemySpawn;
    }

    public void StartEnemySpawn()
    {
        curTime = 0;
        SetEnemiesData();
        AllEnemyUnActive();
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        curTime += Time.deltaTime;
        
        if (curTime > _spawnTime)
        {
            curTime = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].gameObject.activeSelf)
                {
                    continue;
                }
                enemies[i].Init(_baseEnemyData);
                enemies[i].GetComponent<Enemy>().OnEnemyDied += LootManager.Instance.DropCoinAt;
                enemies[i].transform.position = new Vector3(Random.Range(_rend.bounds.min.x, _rend.bounds.max.x), transform.position.y, 0);
                enemies[i].gameObject.SetActive(true);
                break;
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject.activeSelf)
            {
                enemies[i].OnUpdate();
            }
        }
    }

    public void SetEnemiesData()
    {
        for (int i = 0; i < EnemyDataTable.Count; i++)
        {
            if (int.Parse(EnemyDataTable[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            { 
                _baseEnemyData.hp = float.Parse(EnemyDataTable[i]["HP"].ToString());
                _baseEnemyData.moveSpeed = float.Parse(EnemyDataTable[i]["MOVESPEED"].ToString());
                _baseEnemyData.chaseSpeed = float.Parse(EnemyDataTable[i]["CHASESPEED"].ToString());
                _baseEnemyData.coin = int.Parse(EnemyDataTable[i]["COINPERENEMY"].ToString());
                _spawnTime = float.Parse(EnemyDataTable[i]["SPAWNTIME"].ToString());
                break;
            }
        }
    }
    public void AllEnemyUnActive()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
    }


}
