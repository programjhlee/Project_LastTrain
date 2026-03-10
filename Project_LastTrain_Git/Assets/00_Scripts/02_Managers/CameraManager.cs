using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonManager<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera _playerCam;
    [SerializeField] CinemachineVirtualCamera _startCam;

    public void Awake()
    {
        _startCam.Priority = 20;
    }

    public void SetStartCamPriority()
    {
        _startCam.Priority = 0;
    }
}
