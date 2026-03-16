using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    const int DEFAULT_RAY = 0;
    const int IGNORE_RAY = 2;
    [SerializeField] Shield sheild;
    [SerializeField] Transform _lever;
    [SerializeField] Renderer _bodyRend;
    [SerializeField] Renderer _leverRend;

    public void Awake()
    {
        SwitchUnActive();
    }
    public void Interact()
    {
        sheild.TurnOn();
        StartCoroutine(LeverProcess());
        SwitchUnActive();
    }

    public void SwitchActive()
    {
        _bodyRend.material.color = Color.white;
        _leverRend.material.color = Color.white;
        gameObject.layer = DEFAULT_RAY;
        sheild.TurnOff();
    }

    public void SwitchUnActive()
    {
        _leverRend.material.color = Color.black;
        _bodyRend.material.color = Color.black;
        gameObject.layer = IGNORE_RAY;
    }

    public IEnumerator LeverProcess()
    {
        _lever.transform.localRotation = Quaternion.Euler(60f, 0, 0);
        yield return new WaitForSeconds(3f);
        _lever.transform.localRotation = Quaternion.Euler(-60f, 0, 0);
    }


}
