using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, IGravityAffected
{
    LandChecker landChecker;
    float yVelocity = 10;
    public event Action GetCoin;
    public Vector3 spawnDir;

    public void OnEnable()
    {
        spawnDir = UnityEngine.Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
        landChecker = gameObject.AddComponent<LandChecker>();
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
        landChecker.LandCheck();
        
    }

    public void AffectGravity(float gravity)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (!landChecker.IsLanding)
        {
            yVelocity -= gravity * Time.deltaTime;
            transform.position += new Vector3(spawnDir.x * 3f, yVelocity, 0) * Time.deltaTime;
        }
        else
        {
            if (yVelocity < 0)
            {
                yVelocity = 0;
                transform.position = new Vector3(transform.position.x, landChecker.GetLandYPos(), 0);
            }
        }
    }
}
