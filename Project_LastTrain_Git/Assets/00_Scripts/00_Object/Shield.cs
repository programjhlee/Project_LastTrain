using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    WaitForSeconds _shieldTime = new WaitForSeconds(5f);

    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
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
