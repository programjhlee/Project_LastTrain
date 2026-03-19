using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonManager<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera _playerCam;
    [SerializeField] CinemachineVirtualCamera _startCam;
    [SerializeField] CinemachineVirtualCamera _stageClearCam;
    CinemachineVirtualCamera _currentCam;

    public void OnDisable()
    {
        GameManager.Instance.OnTutorialStart -= SetPlayerCamPriority;
        GameManager.Instance.OnStageStart -= SetPlayerCamPriority;
    }
    public void Init()
    {
        _currentCam = _startCam;
        _currentCam.Priority = 20;
        GameManager.Instance.OnTutorialStart += SetPlayerCamPriority;
        GameManager.Instance.OnStageStart += SetPlayerCamPriority;
    }

    public void SetPlayerCamPriority()
    {
        SetCamPriority(_playerCam);
    }

    public void SetStageClearCamPriority()
    {
        SetCamPriority(_stageClearCam);
    }

    public void SetStartCamPrioirty()
    {
        SetCamPriority(_startCam);
    }

    public void SetCamPriority(CinemachineVirtualCamera _virtualCam)
    {
        _currentCam.Priority = 0;
        _currentCam = _virtualCam;
        _currentCam.Priority = 20;
    }
}
