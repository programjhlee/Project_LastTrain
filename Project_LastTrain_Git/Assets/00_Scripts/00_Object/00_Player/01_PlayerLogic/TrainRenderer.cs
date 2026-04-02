using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainRenderer : MonoBehaviour
{
    [SerializeField] TrailRenderer _trailRend;

    public void Awake()
    {
        _trailRend.enabled = false;
    }


    public void TrailRendOn()
    {
        _trailRend.enabled = true;
    }
    public void TrailRendOff()
    {
        _trailRend.enabled = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
