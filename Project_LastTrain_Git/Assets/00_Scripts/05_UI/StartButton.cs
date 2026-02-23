using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    public void ButtonClicked()
    {
        GameManager.Instance.TutorialStart();
        gameObject.SetActive(false);
    }
   
}
