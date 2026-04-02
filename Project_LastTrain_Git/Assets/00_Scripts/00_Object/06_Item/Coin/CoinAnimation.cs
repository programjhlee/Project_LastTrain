using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(0.2f, 1f).SetRelative().SetLoops(-1,LoopType.Yoyo);

    }
    public void Update()
    {
        transform.Rotate(new Vector3(0, 1f, 0));
    }
}
