using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BigEvent : MonoBehaviour
{
    UI_BigEventCaution _ui_caution;
    Renderer _eventRend;
    Vector3 _trainFrontPos;
    float _bigEventSpeed;
    float _damage;


    public float BigEventSpeed
    {
        get
        {
            return _bigEventSpeed;
        }
        set
        {
            _bigEventSpeed = value;
        }
    }

    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    public event Action<float> OnTrainCrashed;
    public event Action OnDestroy;
    public void Init(float speed,Train train)
    {
        gameObject.SetActive(true);
        Renderer trainRend = train.GetComponent<Renderer>();
        _eventRend = GetComponent<Renderer>();
        _damage = 50;
        _bigEventSpeed = speed;
        float arrivedTime = trainRend.bounds.center.x + trainRend.bounds.extents.x + Mathf.Max(speed * 6 - LevelManager.Instance.Level, 3f); 
        transform.position = new Vector3(arrivedTime, trainRend.bounds.center.y, trainRend.bounds.center.z);
        _trainFrontPos = new Vector3(trainRend.bounds.center.x + trainRend.bounds.extents.x, trainRend.bounds.center.y, trainRend.bounds.center.z);
        _ui_caution = UIManager.Instance.ShowUIAt<UI_BigEventCaution>(new Vector2(750, 250));
    }

    public void OnDisable()
    {
        if (_ui_caution != null)
        {
            _ui_caution.Hide();
        }
        OnDestroy?.Invoke();
        OnTrainCrashed = null;
        OnDestroy = null;
    }

    public void Update()
    {
        if (gameObject.activeSelf)
        {
            MoveEvent();
        }
    }



    public void MoveEvent()
    {
        float leftDistance = Vector3.Distance(transform.position, _trainFrontPos);
        _ui_caution.SetDistanceText(leftDistance - _eventRend.bounds.extents.x);
        transform.Translate(-_bigEventSpeed * Time.deltaTime, 0, 0);
    }


    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Train"))
        {
            OnTrainCrashed?.Invoke(_damage);
        }
        gameObject.SetActive(false);
    }

}
