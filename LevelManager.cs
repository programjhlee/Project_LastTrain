using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    int maxLevel = 10;
    public int Level
    {
        get;
        private set;
    }

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Level = 4;
    }


    public void LevelUp()
    {
        if(Level >= maxLevel)
        {
            return;
        }
        Level++;
    }
}
