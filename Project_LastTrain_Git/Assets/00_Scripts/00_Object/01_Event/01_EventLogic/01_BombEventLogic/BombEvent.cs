using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombEvent : Event,ITrainDamageEvent
{
    Player _player;
    [SerializeField] Renderer _rend;
    [SerializeField] AudioClip _bombEnterSound;
    [SerializeField] AudioClip _bombExplosiveSound;
    [SerializeField] GameObject _bombEffect;
    [SerializeField] AudioSource _bombAudio;
    BoxCollider _col;
    UIHUDController _evtHUDController;


    float curTime;
    float curFixAmount;
    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        curTime = 0;
        EventData = initEventData;
        _rend.material.color = Color.white;
        _evtHUDController = GetComponent<UIHUDController>();
        _evtHUDController.Init();
        curFixAmount = EventData.FixAmount;
        _col = GetComponent<BoxCollider>();
        _bombAudio.PlayOneShot(_bombEnterSound);
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
        _col.size = new Vector3(2, 2, 2);
        float warningTime = 0.125f;
        while (curTime <= 1f)
        {
            _rend.material.color = Color.red;
            yield return new WaitForSeconds(warningTime);
            curTime += warningTime;
            _rend.material.color = Color.black;
            yield return new WaitForSeconds(warningTime);
            curTime += warningTime;
        }
        Explosive();
    }

    public override void Exit()
    {
        ReleaseActionEvent();
        OnDamage = null;
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
            Exit();
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out Player p))
        {
            _player = p;
        }

    }
    public void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out Player p))
        {
            _player = null;
        }
    }
    public void Explosive()
    {
        Instantiate(_bombEffect, transform.position, transform.rotation);
        SoundManager.Instance.PlaySFX(_bombExplosiveSound);
        if (_player != null)
        {
            _player.GetComponent<PlayerAction>().TakeDamage(_player.transform.position - transform.position);
        }
        OnDamage?.Invoke(EventData.DamageToTrain);
        Exit();
    }
}
