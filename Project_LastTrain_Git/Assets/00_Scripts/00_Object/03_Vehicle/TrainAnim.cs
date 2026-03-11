using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainAnim : MonoBehaviour
{
    Transform _trainParent;
    Train _train;
    TrainSound _trainSound;
    [SerializeField] GameObject _trainFront;
    [SerializeField] GameObject _trainBack_1;
    [SerializeField] GameObject _trainBack_2;
    [SerializeField] GameObject _trainBack_3;

    float _timerFront;
    float _timerBack1;
    float _timerBack2;
    float _timerBack3;

    float _intervalMin = 8f;
    float _intervalMax = 15f;
    // Start is called before the first frame update
    void Start()
    {
        _trainParent = transform.parent;
        _trainSound = GetComponent<TrainSound>();
        _train = GetComponent<Train>();
        _timerFront = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack1 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack2 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _timerBack3 = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        _train.OnDamaged += OnTakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPaused())
        {
            return;
        }
        TickTimer(_trainFront,ref _timerFront);
        TickTimer(_trainBack_1,ref _timerBack1);
        TickTimer(_trainBack_2,ref _timerBack2);
        TickTimer(_trainBack_3,ref _timerBack3);
    }
    public void TickTimer(GameObject target, ref float timer)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            StartCoroutine(Shake(target, 40f, 0.5f, 0.3f));
            timer = UnityEngine.Random.Range(_intervalMin, _intervalMax);
        }
    }
    IEnumerator Shake(GameObject target, float speed, float shakeForce, float duration)
    {
        Vector3 originRot = target.transform.rotation.eulerAngles;
        float curTime = 0;
        while (curTime < duration)
        {
            float rndZ = Mathf.Sin(curTime * speed) * shakeForce;
            curTime += Time.deltaTime;
            target.transform.rotation = Quaternion.Euler(originRot.x, originRot.y, rndZ * shakeForce);
            yield return null;
        }

        target.transform.rotation = Quaternion.Euler(originRot);


    }

    public void StartTrain()
    {
        StartCoroutine(StartTrainAnim(new Vector3(0, -4, 0), 5f));
    }

    public IEnumerator StartTrainAnim(Vector3 targetPos,float speed)
    {
        _trainSound.PlayTrainStartSound();
        _trainSound.PlayTrainRunningSound();
        while (_trainParent.position.x < 0)
        {
            _trainParent.position = Vector3.Lerp(_trainParent.position, targetPos, speed * Time.deltaTime);
            if(Vector3.Distance(_trainParent.position,targetPos) <= 0.01f)
            {
                _trainParent.position = targetPos;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        CameraManager.Instance.SetStartCamPriority();
        GameManager.Instance.TutorialStart();
    }
    public void OnTakeDamage(float damage)
    {
        float shakeForce = damage / 20;
        StartCoroutine(Shake(_trainFront, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_1, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_2, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_3, 30, shakeForce, 0.5f));
    }

}
