using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonManager<DataManager>
{
    public enum DataTables
    {
        PlatformData,
        EnemyInfoData,
        BigEventSpawnData,
        EnhancePriceData,
        EnhanceValueData,
        EventSpawnData,
        TrainEventData,
        EnemyLevelData,
    }

    string[] _data = Enum.GetNames(typeof(DataTables));

    public List<Dictionary<string,object>> GetData(DataTables dataType)
    {
        string path = _data[(int)dataType];
        if(path != null)
        {
            List<Dictionary<string, object>> returnList = CSVReader.Read(path);
            return returnList;
        }
        return null;
    }
}
