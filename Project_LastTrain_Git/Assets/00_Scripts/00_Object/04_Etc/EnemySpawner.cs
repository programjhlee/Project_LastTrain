using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] UI_HUDValueBarStrategyData _enemyHUDData;
    [SerializeField] GameObject _trainBack;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] List<EnemyData> _enemyDataSOList;

    Renderer _trainRend;
    string[] _enemyName;

    List<Enemy> _activeEnemies;
    List<Enemy> _removeEnemies;


    List<Dictionary<string, object>> _enemyDataTable;
    List<Dictionary<string, object>> _enemyInfoTable;
    
    Dictionary<string, GameObject> _enemyPools;
    Dictionary<string, EnemyData> _enemyDataDics;

    float curTime = 0;
    float _spawnTime;
        
    void Start()
    {
        Init();
    }
    public void Init()
    {
        _enemyDataTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyLevelData);
        _enemyInfoTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyInfoData);
        
        _enemyPools = new Dictionary<string, GameObject>();
        
        _activeEnemies = new List<Enemy>();
        _removeEnemies = new List<Enemy>();
        
        EnemyDataInit();
        _trainRend = _trainBack.GetComponent<Renderer>();

        for (int i = 0; i < _enemyName.Length; i++)
        {
            GameObject enemyPool = new GameObject(_enemyName[i]);
            for (int j = 0; j < 20; j++)
            {
                Enemy enemy = Instantiate(_enemyDataDics[_enemyName[i]].enemyPrefab).GetComponent<Enemy>();
                enemy.name = $"{_enemyName[i]}_{i + 1}";
                enemy.transform.SetParent(enemyPool.transform);
                _enemyPools[_enemyName[i]] = enemyPool;
                enemy.gameObject.SetActive(false);
            }
        }
        GameManager.Instance.OnStageClear += AllEnemyClear;
        GameManager.Instance.OnGameStart += StartEnemySpawn;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= AllEnemyClear;
        GameManager.Instance.OnGameStart -= StartEnemySpawn;
    }

    public void Update()
    {
        if (GameManager.Instance.IsPaused() || GameManager.Instance.IsTutorial())
        {
            return;
        }

        curTime += Time.deltaTime;
        if (curTime > _spawnTime)
        {
            curTime = 0;
            SpawnEnemy();
        }
        EnemyUpdate();
        InActiveEnemyClear();
    }


    public void EnemyDataInit()
    {
        _enemyDataDics = new Dictionary<string, EnemyData>();
        _enemyName = new string[_enemyInfoTable.Count];
        for(int i = 0; i < _enemyInfoTable.Count; i++)
        {
            _enemyName[i] = _enemyDataSOList[i].enemyName;
            _enemyDataSOList[i].maxHp = float.Parse((_enemyInfoTable[i]["HP"].ToString()));
            _enemyDataSOList[i].moveSpeed = float.Parse((_enemyInfoTable[i]["MOVESPEED"].ToString()));
            _enemyDataSOList[i].chaseSpeed = float.Parse((_enemyInfoTable[i]["CHASESPEED"].ToString()));
            _enemyDataSOList[i].coin = int.Parse((_enemyInfoTable[i]["COIN"].ToString()));
            _enemyDataSOList[i].findDistance = float.Parse((_enemyInfoTable[i]["FINDDISTANCE"].ToString()));
            _enemyDataSOList[i].attackDistance = float.Parse((_enemyInfoTable[i]["ATTACKSPEED"].ToString()));
            _enemyDataDics[_enemyName[i]] = _enemyDataSOList[i];
        }

    }


    public void EnemyUpdate()
    {
        for (int i = 0; i < _activeEnemies.Count; i++)
        {
            if (!_activeEnemies[i].gameObject.activeSelf)
            {
                _removeEnemies.Add(_activeEnemies[i]);
            }
            _activeEnemies[i].OnUpdate();
        }
    }

    public void InActiveEnemyClear()
    {
        for (int i = 0; i < _removeEnemies.Count; i++)
        {
            _activeEnemies.Remove(_removeEnemies[i]);
        }
        _removeEnemies.Clear();
    }


    public void SetEnemiesData()
    {
        for (int i = 0; i < _enemyName.Length; i++)
        {
            for (int j = 0; j < _enemyDataTable.Count; j++)
            {
                if (int.Parse(_enemyDataTable[j]["LEVEL"].ToString()) == LevelManager.Instance.Level)
                {
                    _enemyDataDics[_enemyName[i]].maxHp *= float.Parse(_enemyDataTable[i]["HP"].ToString());
                    _enemyDataDics[_enemyName[i]].moveSpeed *= float.Parse(_enemyDataTable[i]["MOVESPEED"].ToString());
                    _enemyDataDics[_enemyName[i]].chaseSpeed *= float.Parse(_enemyDataTable[i]["CHASESPEED"].ToString());
                    _enemyDataDics[_enemyName[i]].coin = int.Parse(_enemyDataTable[i]["COINPERENEMY"].ToString());
                    _spawnTime = float.Parse(_enemyDataTable[i]["SPAWNTIME"].ToString());
                    break;
                }
            }
        }
    }


    public Enemy SpawnEnemy()
    {
        int rnd = Random.Range(0, _enemyName.Length);
        string selectEnemyName = _enemyName[rnd];

        GameObject selectEnemyPool = _enemyPools[selectEnemyName];
        Enemy spawnEnemy = null;
        
        for (int i = 0; i < selectEnemyPool.transform.childCount; i++)
        {
            if (selectEnemyPool.transform.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }
            spawnEnemy = selectEnemyPool.transform.GetChild(i).GetComponent<Enemy>();
            spawnEnemy.Init(_enemyDataDics[selectEnemyName]);
            spawnEnemy.SetEnemyPos(new Vector3(Random.Range(_trainRend.bounds.min.x, _trainRend.bounds.max.x), transform.position.y, 0));
            spawnEnemy.gameObject.SetActive(true);
            _activeEnemies.Add(spawnEnemy);
            break;
        }
        return spawnEnemy;
    }
    public void AllEnemyClear()
    {
        _activeEnemies.Clear();
        _removeEnemies.Clear();
    }
    public void StartEnemySpawn()
    {
        curTime = 0;
        SetEnemiesData();
        AllEnemyClear();
    }
}
