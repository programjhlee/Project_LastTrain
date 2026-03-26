using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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
    public void Awake()
    {
        _evtHUDController = GetComponent<UIHUDController>();
        _col = GetComponent<BoxCollider>();
    }
    public override void Enter(EventData initEventData, float x = 0, float y = 0)
    {
        transform.position = new Vector3(x, y + 10f, 0);
        transform.DOMoveY(y, 0.7f).SetEase(Ease.InQuart).OnComplete(() => transform.DOShakeScale(0.5f));

        _evtHUDController.Init();
        curTime = 0;
        EventData = initEventData;
        _rend.material.color = Color.white;
        curFixAmount = EventData.FixAmount;
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
        transform.DOShakeScale(1f,5f,10);
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
