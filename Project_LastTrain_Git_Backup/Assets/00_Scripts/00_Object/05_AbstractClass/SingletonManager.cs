using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject mgr = new GameObject(typeof(T).Name);
                    _instance = mgr.AddComponent<T>();
                }
            }
            DontDestroyOnLoad(_instance);
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
   
}
