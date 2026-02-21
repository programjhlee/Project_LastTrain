using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonManager<LevelManager>
{
    int maxLevel = 10;
    [SerializeField] PlatformController platformController;
    
    public int Level
    {
        get;
        private set;
    } = 1;
    public void OnEnable()
    {
        platformController.OnArrived += LevelUp;
    }
    public void OnDisable()
    {
        platformController.OnArrived -= LevelUp;
    }
    public void LevelUp()
    {
        Debug.Log("레벨업!!");

        if(Level >= maxLevel)
        {
            Debug.Log("퍼펙트 클리어!");
            return;
        }
        Level++;
    }
    public bool IsMaxLevel()
    {
        return Level >= maxLevel;
    }
}
