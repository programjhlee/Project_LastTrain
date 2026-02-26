using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, IGravityAffected
{
    LandChecker _landChecker;
    float _yVel = 10;
    public event Action GetCoin;
    public Vector3 spawnDir;

    public float YVel { get { return _yVel; } set { _yVel = value; } }

    public Transform TargetTransform { get { return transform; } }

    public LandChecker LandChecker { get { return _landChecker; } }

    public void OnEnable()
    {
        spawnDir = UnityEngine.Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        _landChecker = gameObject.AddComponent<LandChecker>();
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
        _landChecker.LandCheck();
        
    }

    public void AffectGravity(float gravity)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (!_landChecker.IsLanding)
        {
            _yVel -= gravity * Time.deltaTime;
            transform.position += new Vector3(spawnDir.x * 3f, _yVel, 0) * Time.deltaTime;
        }
        else
        {
            if (_yVel < 0)
            {
                _yVel = 0;
                transform.position = new Vector3(transform.position.x, _landChecker.GetLandYPos(), 0);
            }
        }
    }
}
