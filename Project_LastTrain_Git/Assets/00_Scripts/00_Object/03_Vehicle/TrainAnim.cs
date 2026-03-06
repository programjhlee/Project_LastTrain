using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnim : MonoBehaviour
{
    Train _train;
    [SerializeField] GameObject _trainFront;
    [SerializeField] GameObject _trainBack_1;
    [SerializeField] GameObject _trainBack_2;
    [SerializeField] GameObject _trainBack_3;

    float _curTime = 0;
    float _timerFront;
    float _timerBack1;
    float _timerBack2;
    float _timerBack3;

    float _intervalMin = 8f;
    float _intervalMax = 15f;

    // Start is called before the first frame update
    void Start()
    {
        _train = GetComponent<Train>();
        _timerFront = Random.Range(_intervalMin, _intervalMax);
        _timerBack1 = Random.Range(_intervalMin, _intervalMax);
        _timerBack2 = Random.Range(_intervalMin, _intervalMax);
        _timerBack3 = Random.Range(_intervalMin, _intervalMax);
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
            timer = Random.Range(_intervalMin, _intervalMax);
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

    public void OnTakeDamage(float damage)
    {
        float shakeForce = damage / 20;
        StartCoroutine(Shake(_trainFront, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_1, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_2, 30, shakeForce, 0.5f));
        StartCoroutine(Shake(_trainBack_3, 30, shakeForce, 0.5f));
    }

}
