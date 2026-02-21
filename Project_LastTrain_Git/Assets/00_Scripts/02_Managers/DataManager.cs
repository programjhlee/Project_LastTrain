using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonManager<DataManager>
{
    public List<Dictionary<string,object>> GetData(int idx)
    {
        string[] data = Enum.GetNames(typeof(Define.DataTables));
        string path = data[idx];
        if(path != null)
        {
            List<Dictionary<string, object>> returnList = CSVReader.Read(path);
            return returnList;
        }
        return null;
    }
}
