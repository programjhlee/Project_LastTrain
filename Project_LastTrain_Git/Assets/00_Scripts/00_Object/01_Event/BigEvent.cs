using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BigEvent : MonoBehaviour
{
    UI_BigEventCaution ui_caution;
    Renderer eventRend;
    Vector3 trainFrontPos;
    Vector3 eventFrontPos;
    float bigEventSpeed;
    float damage;
    public event Action<float> OnCrashed;



    public void Init(float speed,Train train)
    {
        damage = 50;
        Renderer trainRend = train.GetComponent<Renderer>();
        eventRend = GetComponent<Renderer>();
        bigEventSpeed = speed;
        transform.position = new Vector3(trainRend.bounds.center.x + trainRend.bounds.extents.x + speed * 5f, trainRend.bounds.center.y, trainRend.bounds.center.z);
        trainFrontPos = new Vector3(trainRend.bounds.center.x + trainRend.bounds.extents.x, trainRend.bounds.center.y, trainRend.bounds.center.z);

        OnCrashed += train.TakeDamage;
        ui_caution = UIManager.Instance.ShowUIAt<UI_BigEventCaution>(new Vector2(750, 250));
    }

    public void OnDisable()
    {
        OnCrashed = null;
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
            ui_caution.Hide();
            OnCrashed?.Invoke(damage);
            gameObject.SetActive(false);
        }
    }


}
