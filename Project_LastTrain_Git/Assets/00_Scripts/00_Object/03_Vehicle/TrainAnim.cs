using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainAnim : MonoBehaviour
{
    Transform _trainTransform;
    Train _train;
    [SerializeField] GameObject _trainGameObject;
    [SerializeField] GameObject _trainDestroyEffect;
    [SerializeField] GameObject _trainFront;
    [SerializeField] GameObject _trainBack_1;
    [SerializeField] GameObject _trainBack_2;
    [SerializeField] GameObject _trainBack_3;

    Vector3 _trainFront_OriginPos;
    Vector3 _trainBack_1_OriginPos;
    Vector3 _trainBack_2_OriginPos;
    Vector3 _trainBack_3_OriginPos;


    float _timerFront;
    float _timerBack1;
    float _timerBack2;
    float _timerBack3;

    float _intervalMin = 8f;
    float _intervalMax = 15f;
       
    public void Awake()
    {
        _train = GetComponent<Train>();
        _trainTransform = transform.parent;

        _timerFront = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack1 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack2 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack3 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
    }


    public void OnEnable()
    {
        _train.OnTrainDestroy += OnTrainDestroy;
        _train.OnDamaged += OnTakeDamage;
        _train.OnReset += ResetTrainPos;
        
    }
    public void OnDisable()
    {
        _train.OnTrainDestroy -= OnTrainDestroy;
        _train.OnDamaged -= OnTakeDamage;
        _train.OnReset -= ResetTrainPos;
    }
    public void SetTrainPos(Vector3 pos)
    {
        _trainTransform.position = pos;
    }

    public void ResetTrainPos()
    {
        SetTrainPos(new Vector3(-150f, -4f, 0));
    }
    void Update()
    {
        if (GameManager.Instance.IsPaused() || !_train.IsRunning)
        {
            return;
        }
        Trumbling();
        TickTimer(_trainFront,ref _timerFront);
        TickTimer(_trainBack_1,ref _timerBack1);
        TickTimer(_trainBack_2,ref _timerBack2);
        TickTimer(_trainBack_3,ref _timerBack3);
    }
    public void TickTimer(GameObject target, ref float timer)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            StartCoroutine(Shake(target, 40f, 0.5f, 0.3f));
            timer = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        }
    }

    public void Trumbling(float strength = 0.015f)
    {
        float rndFloatX = UnityEngine.Random.Range(-strength, strength);
        float rndFloatY = UnityEngine.Random.Range(-strength, strength);
        
        _trainFront.transform.position = _trainFront_OriginPos + new Vector3(rndFloatX, rndFloatY, 0);
        _trainBack_1.transform.position = _trainBack_1_OriginPos +  new Vector3(rndFloatX, rndFloatY, 0);
        _trainBack_2.transform.position = _trainBack_2_OriginPos + new Vector3(rndFloatX, rndFloatY, 0);
        _trainBack_3.transform.position = _trainBack_3_OriginPos + new Vector3(rndFloatX, rndFloatY, 0);
    }
    IEnumerator Shake(GameObject target, float speed, float shakeForce, float duration)
    {
        Vector3 originRot = target.transform.rotation.eulerAngles;
        float curTime = 0;
        while (curTime < duration)
        {
            float rndZ = Mathf.Sin(curTime * speed) * shakeForce;
            curTime += Time.deltaTime;
            target.transform.rotation = Quaternion.Euler(originRot.x, originRot.y, rndZ * shakeForce);
            yield return null;
        }

        target.transform.rotation = Quaternion.Euler(originRot);


    }
    public IEnumerator StartTrainAnim(Vector3 targetPos,float speed)
    {
        while (_trainTransform.position.x < 0)
        {
            _trainTransform.position = Vector3.Lerp(_trainTransform.position, targetPos, speed * Time.deltaTime);
            if(Vector3.Distance(_trainTransform.position,targetPos) <= 0.01f)
            {
                _trainTransform.position = targetPos;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        _trainFront_OriginPos = _trainFront.transform.position;
        _trainBack_1_OriginPos = _trainBack_1.transform.position;
        _trainBack_2_OriginPos = _trainBack_2.transform.position;
        _trainBack_3_OriginPos = _trainBack_3.transform.position;
    }
    public void OnTakeDamage(float damage)
    {
        float shakeForce = damage / 20;
        StartCoroutine(Shake(_trainFront, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_1, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_2, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_3, 30, shakeForce, 0.5f));
    }

    public void OnTrainDestroy()
    {
        StartCoroutine(DestroyProcess());
    }

    IEnumerator DestroyProcess()
    {
        CameraManager.Instance.SetTrainCamPriority();
        CameraManager.Instance.CamShake();
        yield return new WaitForSeconds(2f);
        CameraManager.Instance.StopShake();
        Instantiate(_trainDestroyEffect, _trainFront.transform.position, _trainFront.transform.rotation);
        Instantiate(_trainDestroyEffect, _trainBack_1.transform.position, _trainBack_1.transform.rotation);
        Instantiate(_trainDestroyEffect, _trainBack_2.transform.position, _trainBack_2.transform.rotation);
        Instantiate(_trainDestroyEffect, _trainBack_3.transform.position, _trainBack_3.transform.rotation);
        _trainGameObject.SetActive(false);
    }
}
