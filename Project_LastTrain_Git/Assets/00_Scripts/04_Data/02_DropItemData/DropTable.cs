using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="DropTable", menuName = "Create DropTable")]
public class DropTable: ScriptableObject
{

    [System.Serializable]
    public class DropEntry
    {
        public Item Item;
        public float DropChance;
    }

    [SerializeField] int _mapID;
    [SerializeField] string _mapName;
    [SerializeField] List<DropEntry> _dropItems;

    public int MapID => _mapID;
    public string MapName => _mapName;
    public List<DropEntry> DropItems => _dropItems;
}
