using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] UI_Title _uiTitle;
    public void ButtonClicked()
    {
        _uiTitle.Hide();
        gameObject.SetActive(false);
        GameManager.Instance.TutorialStart();
    }
   
}
