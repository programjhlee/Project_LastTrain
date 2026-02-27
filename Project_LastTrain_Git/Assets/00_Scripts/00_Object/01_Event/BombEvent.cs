using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombEvent : Event,ITrainDamageEvent
{
    Player player;
    Renderer rend;
    BoxCollider col;
    UIHUDStack _evtHUDController;
    UI_HUDValueBar _evtHUDFixValueBar;


    float curTime;
    float curFixAmount;
    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        base.Enter(initEventData);
        _evtHUDController = GetComponent<UIHUDStack>();
        rend = GetComponent<Renderer>();
        curFixAmount = eventData.fixAmount;
        col = GetComponent<BoxCollider>();
    }

    public override void Execute()
    {
        curTime += Time.deltaTime;
        _evtHUDController.UpdateUIHUDPos();
        if (curTime > eventData.cyclePerTime)
        {
            curTime = 0;
            Exit();
        }

    }

    IEnumerator ExplosiveProcess()
    {
        col.size = new Vector3(2, 2, 2);
        Color original = rend.material.color;
        float warningTime = 0.125f;
        while (curTime <= 1f)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(warningTime);
            curTime += warningTime;
            rend.material.color = original;
            yield return new WaitForSeconds(warningTime);
            curTime += warningTime;
        }
        if(player != null)
        {
            player.GetComponent<PlayerAction>().TakeDamage(player.transform.position - transform.position);
        }
        OnDamage?.Invoke(eventData.damageToTrain);
        Explosive();
    }

    public override void Exit()
    {
        StartCoroutine(ExplosiveProcess());
    }

    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        InvokeTakeFix(curFixAmount / eventData.fixAmount);
        if (curFixAmount <= 0)
        {
            InvokeOnFix();
            Fixed();
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out Player p))
        {
            player = p;
        }

    }
    public void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out Player p))
        {
            player = null;
        }
    }
    public void Fixed()
    {
        ReleaseActionEvent();
        OnDamage = null;
        Destroy(gameObject);
    }
    public void Explosive()
    {
        ReleaseActionEvent();
        _evtHUDController.UIHUDListClear();
        Debug.Log("ลอมü!");
        Destroy(gameObject);
    }
}
