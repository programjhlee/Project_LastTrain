using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombEvent : Event,ITrainDamageEvent
{
    Player player;
    Renderer rend;
    BoxCollider col;
    UIHUDController _evtHUDController;


    float curTime;
    float curFixAmount;
    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        curTime = 0;
        EventData = initEventData;
        _evtHUDController = GetComponent<UIHUDController>();
        _evtHUDController.Init();
        rend = GetComponent<Renderer>();
        curFixAmount = EventData.FixAmount;
        col = GetComponent<BoxCollider>();
    }

    public override void Execute()
    {
        curTime += Time.deltaTime;
        _evtHUDController.UpdateUIHUDPos();
        if (curTime > EventData.CyclePerTime)
        {
            curTime = 0;
            StartCoroutine(ExplosiveProcess());
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
        OnDamage?.Invoke(EventData.DamageToTrain);
        Explosive();
    }

    public override void Exit()
    {
        InvokeOnFix();
        ReleaseActionEvent();
        _evtHUDController.UIHUDListClear();
        gameObject.SetActive(false);
    }

    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        InvokeTakeFix(curFixAmount / EventData.FixAmount);
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
        Exit();
    }
    public void Explosive()
    {
        ReleaseActionEvent();
        Exit();
    }
}
