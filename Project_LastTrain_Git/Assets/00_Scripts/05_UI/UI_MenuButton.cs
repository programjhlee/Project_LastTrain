using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuButton : MonoBehaviour
{
    [SerializeField] Button _uiMenuButton;


    public void MenuButtonClicked()
    {
        UI_Menu uiMenu = UIManager.Instance.ShowPopupUIAt<UI_Menu>(Vector3.zero);
        GameManager.Instance.GamePaused();
        uiMenu.OnMenuClosed += UIClosed;
        _uiMenuButton.interactable = false;

    }
    public void UIClosed()
    {
        GameManager.Instance.GameResume();
        _uiMenuButton.interactable = true;
    }
}
