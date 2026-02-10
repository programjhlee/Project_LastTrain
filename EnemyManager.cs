using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy testEnemy;
    [SerializeField] EnemyData baseEnemyData;
    List<Dictionary<string, object>> EnemyDataTable;
    float _spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        EnemyDataTable = DataManager.instance.GetData((int)Define.DataTables.EnemyLevelData);
        SetEnemiesData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnemiesData()
    {
        for (int i = 0; i < EnemyDataTable.Count; i++)
        {
            if (int.Parse(EnemyDataTable[i]["LEVEL"].ToString()) == LevelManager.instance.Level)
            { 
                baseEnemyData.hp = float.Parse(EnemyDataTable[i]["HP"].ToString());
                baseEnemyData.moveSpeed = float.Parse(EnemyDataTable[i]["MOVESPEED"].ToString());
                baseEnemyData.chaseSpeed = float.Parse(EnemyDataTable[i]["CHASESPEED"].ToString());
                baseEnemyData.coin = int.Parse(EnemyDataTable[i]["COINPERENEMY"].ToString());
                testEnemy.Init(baseEnemyData);
            }
        }
    }


}
