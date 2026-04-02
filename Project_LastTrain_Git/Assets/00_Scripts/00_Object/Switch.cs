using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    const int DEFAULT_RAY = 0;
    const int IGNORE_RAY = 2;

    [SerializeField] UI_HUDControlGuide _uiControlGuide;
    [SerializeField] UI_HUDControlGuideStrategyData _uiControlGuideStrategyData;
    [SerializeField] AudioClip _switchSoundEffect;
    [SerializeField] Shield sheild;
    [SerializeField] Transform _lever;
    [SerializeField] Renderer _bodyRend;
    [SerializeField] Renderer _leverRend;



    public void Awake()
    {
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_HUDControlGuide>(transform);
        _uiControlGuide.BindData(_uiControlGuideStrategyData);
        SwitchUnActive();
    }
    public void Interact()
    {
        sheild.TurnOn();
        SoundManager.Instance.PlaySFX(_switchSoundEffect);
        StartCoroutine(LeverProcess());
        SwitchUnActive();
    }

    public void SwitchActive() 
    {
        _uiControlGuide.Show();
        _bodyRend.material.color = Color.white;
        _leverRend.material.color = Color.white;
        gameObject.layer = DEFAULT_RAY;
        sheild.TurnOff();
    }

    public void Update()
    {
        _uiControlGuide.UpdatePos();
    }

    public void SwitchUnActive()
    {
        _leverRend.material.color = Color.black;
        _bodyRend.material.color = Color.black;
        gameObject.layer = IGNORE_RAY;
        _uiControlGuide.Hide();
    }

    public IEnumerator LeverProcess()
    {
        _lever.transform.localRotation = Quaternion.Euler(60f, 0, 0);
        yield return new WaitForSeconds(3f);
        _lever.transform.localRotation = Quaternion.Euler(-60f, 0, 0);
    }


}
