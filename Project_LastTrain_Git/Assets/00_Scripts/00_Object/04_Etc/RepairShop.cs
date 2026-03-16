using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RepairShop : MonoBehaviour
{
    [SerializeField] Vector3 _repairShopPos;

    public void RepairShopInit()
    {
        transform.position = _repairShopPos;
        gameObject.SetActive(false);
    }

    public void RepairShopMoveAt(Vector3 targetPos)
    {
        StartCoroutine(MoveProcess(targetPos));
    }

    public void RepairShopArrived()
    {
        gameObject.SetActive(true);
        GameManager.Instance.OnStageStart += TrainStart;
        StartCoroutine(MoveProcess(new Vector3(36f, transform.position.y, transform.position.z)));
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageStart -= TrainStart;
    }
    public void TrainStart()
    {
        StartCoroutine(MoveProcess(new Vector3(-70f, transform.position.y, transform.position.z),() =>
        {
            if (transform.position.x < -60f)
            {
                transform.position = _repairShopPos;
                GameManager.Instance.OnGameStart -= TrainStart;
                gameObject.SetActive(false);
            }
        }
        ));
        
    }

    public IEnumerator MoveProcess(Vector3 targetPos,Action OnComplete = null, float speed = 2)
    {
        while (Vector3.Distance(transform.position, targetPos) >= 0.01f) 
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        OnComplete?.Invoke();
    }

}
