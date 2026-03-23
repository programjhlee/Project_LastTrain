using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : SingletonManager<LevelManager>
{
    int maxLevel = 2;
    [SerializeField] PlatformController platformController;

    public event Action OnLevelChanged;
    public event Action OnAllLevelClear;


    public int Level
    {
        get;
        private set;
    } = 1;
    public void OnEnable()
    {
        GameManager.Instance.OnStageClear += LevelUp;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= LevelUp;
    }


    public void ResetLevel()
    {
        Level = 1;
    }

    public void LevelUp()
    {
        if(Level >= maxLevel)
        {
            Debug.Log("모든 레벨 클리어! ");
            OnAllLevelClear?.Invoke();
            return;
        }
        Level++;
        OnLevelChanged?.Invoke();
    }
    public bool IsMaxLevel()
    {
        return Level >= maxLevel;
    }
}
