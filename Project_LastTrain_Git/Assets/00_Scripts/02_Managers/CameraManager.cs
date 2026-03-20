using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : SingletonManager<CameraManager>
{
    [SerializeField] CinemachineBrain _brain;
    [SerializeField] CinemachineVirtualCamera _playerCam;
    [SerializeField] CinemachineVirtualCamera _startCam;
    [SerializeField] CinemachineVirtualCamera _stageClearCam;
    [SerializeField] CinemachineVirtualCamera _allClearCam;
    CinemachineVirtualCamera _currentCam;

    public void Awake()
    {
        _currentCam = _startCam;
        _currentCam.Priority = 20;
    }
    public void OnEnable()
    {
        GameManager.Instance.OnTutorialStart += SetPlayerCamPriority;
        GameManager.Instance.OnStageStart += SetPlayerCamPriority;

    }
    public void OnDisable()
    {
        GameManager.Instance.OnTutorialStart -= SetPlayerCamPriority;
        GameManager.Instance.OnStageStart -= SetPlayerCamPriority;
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

    public void AllClearCamProcess()
    {
        _brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        SetCamPriority(_allClearCam);
        _allClearCam.transform.DOMoveX(-55f, 0.5f).SetEase(Ease.OutBack);
    }
    public void SetCamPriority(CinemachineVirtualCamera _virtualCam)
    {
        _currentCam.Priority = 0;
        _currentCam = _virtualCam;
        _currentCam.Priority = 20;
    }
}
