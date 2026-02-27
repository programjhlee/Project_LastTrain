using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, IGravityAffected
{
    CollideChecker _CollideChecker;
    float _yVel = 10;
    public event Action GetCoin;
    public Vector3 spawnDir;

    public float YVel { get { return _yVel; } set { _yVel = value; } }

    public Transform TargetTransform { get { return transform; } }

    public CollideChecker CollideChecker { get { return _CollideChecker; } }

    public void OnEnable()
    {
        spawnDir = UnityEngine.Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        _CollideChecker = gameObject.AddComponent<CollideChecker>();
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GetCoin?.Invoke();
        }
    }
    public void OnUpdate()
    {
        transform.position += new Vector3(0, _yVel, 0) * Time.deltaTime;
        _CollideChecker.LandCheck();
    }
}
