using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    
    Button btn;
    // Start is called before the first frame update
    
    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(NextLevel);
    }
    void OnDestroy()
    {
        btn.onClick.RemoveListener(NextLevel);
    }
    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        GameManager.Instance.GameStart();
        UIManager.Instance.HideUI<UI_Enhance>();        
    }
}
