using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonManager<DataManager>
{
    string[] _data = Enum.GetNames(typeof(Define.DataTables));

    public List<Dictionary<string,object>> GetData(int idx)
    {
        string path = _data[idx];
        if(path != null)
        {
            List<Dictionary<string, object>> returnList = CSVReader.Read(path);
            return returnList;
        }
        return null;
    }
}
