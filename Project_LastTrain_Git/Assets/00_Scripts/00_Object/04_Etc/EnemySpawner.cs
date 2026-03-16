using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] UI_HUDValueBarStrategyData _enemyHUDData;
    [SerializeField] GameObject _trainBack;
    [SerializeField] List<EnemyData> _enemyDataSOList;

    Collider _trainCol;
    int[] _enemyID;

    List<Enemy> _activeEnemies;
    List<Enemy> _removeEnemies;

    List<Dictionary<string, object>> _enemyLevelTable;
    List<Dictionary<string, object>> _enemyInfoTable;
    
    Dictionary<int, GameObject> _enemyPools;
    Dictionary<int, EnemyData> _enemyDataDics;

    float curTime = 0;
    float _spawnTime;
        
    void Start()
    {
        Init();
    }
    public void Init()
    {
        _enemyLevelTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyLevelData);
        _enemyInfoTable = DataManager.Instance.GetData((int)Define.DataTables.EnemyInfoData);
        
        _enemyPools = new Dictionary<int, GameObject>();
        
        _activeEnemies = new List<Enemy>();
        _removeEnemies = new List<Enemy>();
        
        EnemyDataInit();
        
        _trainCol = _trainBack.GetComponent<Collider>();

        for (int i = 0; i < _enemyID.Length; i++)
        {
            GameObject enemyPool = new GameObject(_enemyDataDics[_enemyID[i]].enemyName);
            for (int j = 0; j < 20; j++)
            {
                Enemy enemy = Instantiate(Resources.Load<GameObject>(_enemyDataDics[_enemyID[i]].prefabAddress)).GetComponent<Enemy>();
                enemy.name = $"{_enemyDataDics[_enemyID[i]].enemyName}_{j + 1}";
                enemy.transform.SetParent(enemyPool.transform);
                _enemyPools[_enemyID[i]] = enemyPool;
                enemy.OnAwake();
                enemy.gameObject.SetActive(false);
            }
        }
        SetEnemiesData();
        GameManager.Instance.OnStageClear += AllEnemyClear;
        LevelManager.Instance.OnLevelChanged += SetEnemiesData;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= AllEnemyClear;
        LevelManager.Instance.OnLevelChanged -= SetEnemiesData;
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
            SpawnEnemy();
        }
        EnemyUpdate();
        InActiveEnemyClear();
    }

    public void LateUpdate()
    {
        for (int i = 0; i < _activeEnemies.Count; i++)
        {
            if (!_activeEnemies[i].gameObject.activeSelf)
            {
                _removeEnemies.Add(_activeEnemies[i]);
            }
            _activeEnemies[i].OnLateUpdate();
        }
    }
    public void EnemyDataInit()
    {
        _enemyDataDics = new Dictionary<int, EnemyData>();
        _enemyID = new int[_enemyInfoTable.Count];

        for(int i = 0; i < _enemyInfoTable.Count; i++)
        {
            _enemyID[i] = int.Parse(_enemyInfoTable[i]["ENEMYID"].ToString());
            _enemyDataSOList[i].enemyID = _enemyID[i];
            _enemyDataSOList[i].maxHp = float.Parse((_enemyInfoTable[i]["HP"].ToString()));
            _enemyDataSOList[i].moveSpeed = float.Parse((_enemyInfoTable[i]["MOVESPEED"].ToString()));
            _enemyDataSOList[i].chaseSpeed = float.Parse((_enemyInfoTable[i]["CHASESPEED"].ToString()));
            _enemyDataSOList[i].findDistance = float.Parse((_enemyInfoTable[i]["FINDDISTANCE"].ToString()));
            _enemyDataSOList[i].attackDistance = float.Parse((_enemyInfoTable[i]["ATTACKSPEED"].ToString()));
            _enemyDataSOList[i].prefabAddress = _enemyInfoTable[i]["ADDRESS"].ToString();
            _enemyDataDics[_enemyID[i]] = _enemyDataSOList[i];
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
        for (int i = 0; i < _enemyID.Length; i++)
        {
            for (int j = 0; j < _enemyLevelTable.Count; j++)
            {
                if (int.Parse(_enemyLevelTable[j]["LEVEL"].ToString()) == LevelManager.Instance.Level)
                {
                    _enemyDataDics[_enemyID[i]].maxHp *= float.Parse(_enemyLevelTable[j]["HP"].ToString());
                    _enemyDataDics[_enemyID[i]].moveSpeed *= float.Parse(_enemyLevelTable[j]["MOVESPEED"].ToString());
                    _enemyDataDics[_enemyID[i]].chaseSpeed *= float.Parse(_enemyLevelTable[j]["CHASESPEED"].ToString());
                    _spawnTime = float.Parse(_enemyLevelTable[j]["SPAWNTIME"].ToString());
                    break;
                }
            }
        }
    }


    public Enemy SpawnEnemy()
    {
        int rnd = Random.Range(0, _enemyID.Length);
        int selectEnemyID = _enemyID[rnd];

        GameObject selectEnemyPool = _enemyPools[selectEnemyID];
        Enemy spawnEnemy = null;
        
        for (int i = 0; i < selectEnemyPool.transform.childCount; i++)
        {
            if (selectEnemyPool.transform.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }
            spawnEnemy = selectEnemyPool.transform.GetChild(i).GetComponent<Enemy>();
            spawnEnemy.Init(_enemyDataDics[selectEnemyID]);
            spawnEnemy.SetEnemyPos(new Vector3(Random.Range(_trainCol.bounds.min.x, _trainCol.bounds.max.x), transform.position.y, 0));
            spawnEnemy.gameObject.SetActive(true);
            _activeEnemies.Add(spawnEnemy);
            break;
        }
        return spawnEnemy;
    }
    public void AllEnemyClear()
    {
        curTime = 0;

        for(int i = 0; i<_activeEnemies.Count; i++)
        {
            _activeEnemies[i].OnDespawn();
        }

        _activeEnemies.Clear();
        _removeEnemies.Clear();
    }
}
