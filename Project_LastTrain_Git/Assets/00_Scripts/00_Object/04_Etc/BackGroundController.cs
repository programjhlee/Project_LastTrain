using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackGroundController : MonoBehaviour
{
    PlatformController _platformController;
    [SerializeField] GameObject _railPrefab;
    [SerializeField] GameObject _backGroundPrefab;
    List<GameObject> _backGrounds;
    List<GameObject> _rails;
    float _railSizeX;
    float _railStartPosX = -120f;

    float _backGroundSpeed;
    float _railSpeed;
    public void Init()
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
    }

    public void SpeedInit()
    {
        _backGroundSpeed = _platformController.TrainSpeed * 2;
        _railSpeed = _platformController.TrainSpeed * 50;
    }

    public void OnUpdate()
    {
        BackGroundControl();
        RailControl();
    }

    public void RailControl()
    {
        for (int i = 0; i < _rails.Count; i++)
        {
            if (_rails[i].transform.position.x < -_railSizeX * 3)
            {
                _rails[i].transform.position = _rails[_rails.Count - 1].transform.position + new Vector3(_railSizeX, 0, 0);
                _rails.Add(_rails[i]);
                _rails.RemoveAt(i);
            }

            _rails[i].transform.position += Vector3.left * _railSpeed * Time.deltaTime;

        }
    }

    public void BackGroundControl()
    {
        for (int i = 0; i < _backGrounds.Count; i++)
        {
            if (_backGrounds[i].transform.position.x < -570f)
            {
                _backGrounds[i].transform.position = new Vector3(770f, 0, 0);
                _backGrounds.Add(_backGrounds[i]);
                _backGrounds.RemoveAt(i);
            }

            _backGrounds[i].transform.position += Vector3.left * _backGroundSpeed * Time.deltaTime;
        }
    }

    public IEnumerator ArrivedAnimationProcess(Action OnComplete)
    {
        while (_backGroundSpeed > 0 || _railSpeed > 0)
        {
            OnUpdate();
            if(_backGroundSpeed <= 0.01f)
            {
                _backGroundSpeed = 0;
            }
            if(_railSpeed <= 0.01f)
            {
                _railSpeed = 0;
            }

            _backGroundSpeed = Mathf.Lerp(_backGroundSpeed, 0, 2f * Time.deltaTime);
            _railSpeed = Mathf.Lerp(_railSpeed, 0, 2f * Time.deltaTime);

            Debug.Log(_backGroundSpeed);
            Debug.Log(_railSpeed);
            yield return null;
        }
        OnComplete?.Invoke();

    }
}
