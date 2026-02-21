using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    const int DEFAULT_RAY = 0;
    const int IGNORE_RAY = 2;
    [SerializeField] Shield sheild;
    Renderer _rend;

    public void Awake()
    {
        _rend = GetComponent<Renderer>();
        SwitchUnActive();
    }
    public void Interact()
    {
        sheild.TurnOn();
        SwitchUnActive();
    }

    public void SwitchActive()
    {
        _rend.material.color = Color.red;
        gameObject.layer = DEFAULT_RAY; 
    }

    public void SwitchUnActive()
    {
        _rend.material.color = Color.gray;
        gameObject.layer = IGNORE_RAY;
    }

}
