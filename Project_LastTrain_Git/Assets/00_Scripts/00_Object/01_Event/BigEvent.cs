using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BigEvent : MonoBehaviour
{
    UI_BigEventCaution ui_caution;
    Renderer eventRend;
    Vector3 trainFrontPos;
    float bigEventSpeed;
    float damage;


    public event Action<float> OnTrainCrashed;
    public event Action OnDestroy;



    public void Init(float speed,Train train)
    {
        Renderer trainRend = train.GetComponent<Renderer>();
        Debug.Log(train);
        eventRend = GetComponent<Renderer>();
        damage = 50;
        bigEventSpeed = speed;
        float arrivedTime = trainRend.bounds.center.x + trainRend.bounds.extents.x + Mathf.Max(speed * 6 - LevelManager.Instance.Level, 3f); 
        transform.position = new Vector3(arrivedTime, trainRend.bounds.center.y, trainRend.bounds.center.z);
        trainFrontPos = new Vector3(trainRend.bounds.center.x + trainRend.bounds.extents.x, trainRend.bounds.center.y, trainRend.bounds.center.z);
        ui_caution = UIManager.Instance.ShowUIAt<UI_BigEventCaution>(new Vector2(750, 250));
    }

    public void OnDisable()
    {
        if (ui_caution != null)
        {
            ui_caution.Hide();
        }
        OnTrainCrashed = null;
        OnDestroy = null;
    }

    public void Update()
    {
        if (gameObject.activeSelf)
        {
            float leftDistance = Vector3.Distance(transform.position, trainFrontPos);
            ui_caution.SetDistanceText(leftDistance - eventRend.bounds.extents.x);
            transform.Translate(-bigEventSpeed * Time.deltaTime, 0, 0);
        }
    }


    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Train"))
        {
            OnTrainCrashed?.Invoke(damage);
        }
        ui_caution.Hide();
        OnDestroy?.Invoke();
        gameObject.SetActive(false);
    }


}
