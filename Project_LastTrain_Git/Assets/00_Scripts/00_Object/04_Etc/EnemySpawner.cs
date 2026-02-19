using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();
    [SerializeField] Train train;
    [SerializeField]GameObject enemyPrefab;
    [SerializeField] EnemyData baseEnemyData;
    List<Dictionary<string, object>> EnemyDataTable;

    float curTime = 0;
    float _spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    public void Init()
    {
        EnemyDataTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyLevelData);
        SetEnemiesData();
        GameObject enemyPool = new GameObject("EnemyPool");
        for (int i = 0; i < 50; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.name = $"Enemy_{i + 1}";
            enemy.transform.SetParent(enemyPool.transform);
            enemies.Add(enemy.GetComponent<Enemy>());
            enemy.SetActive(false);
            GravityManager.Instance.AddGravityObj(enemy.GetComponent<IGravityAffected>());
        }
        GameManager.Instance.OnStageClear += AllEnemyUnActive;
        GameManager.Instance.OnGameStart += OnStart;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= AllEnemyUnActive;
        GameManager.Instance.OnGameStart -= OnStart;
    }


    public void AllEnemyUnActive()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
    }



    public void OnStart()
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
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].gameObject.activeSelf)
                {
                    continue;
                }
                enemies[i].GetComponent<Enemy>().OnEnemyDied += LootManager.Instance.DropCoinAt;
                enemies[i].gameObject.SetActive(true);
                enemies[i].Init(baseEnemyData);
                enemies[i].transform.position = new Vector3(Random.Range(train.GetComponent<Collider>().bounds.min.x, train.GetComponent<Collider>().bounds.max.x), transform.position.y, 0);
                curTime = 0;
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
                baseEnemyData.hp = float.Parse(EnemyDataTable[i]["HP"].ToString());
                baseEnemyData.moveSpeed = float.Parse(EnemyDataTable[i]["MOVESPEED"].ToString());
                baseEnemyData.chaseSpeed = float.Parse(EnemyDataTable[i]["CHASESPEED"].ToString());
                baseEnemyData.coin = int.Parse(EnemyDataTable[i]["COINPERENEMY"].ToString());
                _spawnTime = float.Parse(EnemyDataTable[i]["SPAWNTIME"].ToString());
                break;
            }
        }
    }


}
