using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackGroundController : MonoBehaviour
{
    PlatformController _platformController;
    [SerializeField] GameObject _railPrefab;
    [SerializeField] GameObject _backGroundPrefab;
    [SerializeField] GameObject _backPropPrefab;
    [SerializeField] RepairShop _repairShop;
    List<GameObject> _backGrounds;
    List<GameObject> _backProps;
    List<GameObject> _rails;

    float _railSizeX;
    float _railStartPosX = -120f;

    float _backGroundSpeed;
    float _railSpeed;


    public void Awake()
    {
        _platformController = GetComponent<PlatformController>();
        _rails = new List<GameObject>();
        _backGrounds = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            GameObject rail = Instantiate(_railPrefab);
            
            rail.transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
            rail.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            _railSizeX = rail.GetComponentInChildren<Renderer>().bounds.extents.x * 2;
            rail.transform.position = new Vector3(_railStartPosX + i * _railSizeX, -10.1f, 0);
            _rails.Add(rail);
            
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject backGround = Instantiate(_backGroundPrefab);
            backGround.transform.position = new Vector3(770f * i, 0, 0);
            _backGrounds.Add(backGround);
        }
        _repairShop.RepairShopInit();
    }

    public void OnEnable()
    {
        _platformController.OnReset += ResetBackGroundPos;
    }
    public void OnDisable()
    {
        _platformController.OnReset -= ResetBackGroundPos;
    }
    public void SetBackGroundSpeed()
    {
        _backGroundSpeed = _platformController.TrainSpeed * 2;
        _railSpeed = _platformController.TrainSpeed * 50;
    }

    public void OnUpdate()
    {
        BackGroundControl(_backGroundSpeed);
        RailControl(_railSpeed);
    }
    public void RailControl(float speed)
    {
        for (int i = 0; i < _rails.Count; i++)
        {
            if (_rails[i].transform.position.x < -_railSizeX * 3)
            {
                _rails[i].transform.position = _rails[_rails.Count - 1].transform.position + new Vector3(_railSizeX, 0, 0);
                _rails.Add(_rails[i]);
                _rails.RemoveAt(i);
            }

            _rails[i].transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public void BackGroundControl(float speed )
    {
        for (int i = 0; i < _backGrounds.Count; i++)
        {
            if (_backGrounds[i].transform.position.x < -570f)
            {
                _backGrounds[i].transform.position = new Vector3(770f, 0, 0);
                _backGrounds.Add(_backGrounds[i]);
                _backGrounds.RemoveAt(i);
            }

            _backGrounds[i].transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public IEnumerator ArrivedAnimationProcess(Action OnComplete)
    {
        float tempBackGroundSpeed = _backGroundSpeed;
        float tempRailSpeed = _railSpeed;
        _repairShop.RepairShopArrived();
        CameraManager.Instance.SetStageClearCamPriority();
        while (tempBackGroundSpeed > 0 || tempRailSpeed > 0)
        {

            BackGroundControl(tempBackGroundSpeed);
            RailControl(tempRailSpeed);
            if (tempBackGroundSpeed <= 0.01f)
            {
                tempBackGroundSpeed = 0;
            }
            if(tempRailSpeed <= 0.01f)
            {
                tempRailSpeed = 0;
            }
            _repairShop.transform.position = Vector3.Lerp(_repairShop.transform.position, new Vector3(36f, -6.7f, 8.3f),2f * Time.deltaTime);
            tempBackGroundSpeed = Mathf.Lerp(tempBackGroundSpeed, 0, 2f * Time.deltaTime);
            tempRailSpeed = Mathf.Lerp(tempRailSpeed, 0, 2f * Time.deltaTime);
            yield return null;
        }
        OnComplete?.Invoke();

    }

    public void ResetBackGroundPos()
    {
        for (int i = 0; i < _rails.Count; i++)
        {
            _rails[i].transform.position = new Vector3(_railStartPosX + i * _railSizeX, -10.1f, 0);
        }
        for (int i = 0; i < _backGrounds.Count; i++)
        {
            _backGrounds[i].transform.position = new Vector3(770f * i, 0, 0);
        }
    }
}
