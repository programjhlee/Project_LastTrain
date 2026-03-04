using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.Log("파괴됨! " + gameObject.name);
        Debug.Log(System.Environment.StackTrace);  // 어디서 호출됐는지 출력
    }
}
