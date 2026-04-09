using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : SingletonManager<LevelManager>
{
    int _maxLevel = 5;
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
        if(Level >= _maxLevel)
        {
            OnAllLevelClear?.Invoke();
            return;
        }
        Level++;
        OnLevelChanged?.Invoke();
    }
    public bool IsMaxLevel()
    {
        return Level >= _maxLevel;
    }
}
