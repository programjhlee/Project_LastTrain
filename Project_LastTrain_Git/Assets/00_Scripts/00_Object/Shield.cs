using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject _shieldEffect;
    WaitForSeconds _shieldTime = new WaitForSeconds(5f);

    public void Start()
    {
        gameObject.SetActive(false);
        _shieldEffect.SetActive(false);
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
        _shieldEffect.SetActive(true);
        StartCoroutine(ShieldProcess(_shieldTime));
    }
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
    IEnumerator ShieldProcess(WaitForSeconds shieldTime)
    {
        yield return _shieldTime;
        TurnOff();
    }
}
