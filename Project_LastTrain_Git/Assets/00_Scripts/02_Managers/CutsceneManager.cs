using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CutsceneManager : SingletonManager<CutsceneManager>
{
    [SerializeField] Canvas _canvas;
    [SerializeField]List<Cutscene> _cutsceneList;
    Cutscene _currentScene;

    Dictionary<CutsceneType, Cutscene> _cutsceneDict;
    public enum CutsceneType
    {
        GameClear,
        GameOver
    }

    public void Awake()
    {
        _cutsceneDict = new Dictionary<CutsceneType, Cutscene>();
        for(int i = 0; i < _cutsceneList.Count; i++)
        {
            Cutscene instance = Instantiate(_cutsceneList[i].gameObject,Vector3.zero,Quaternion.identity).GetComponent<Cutscene>();
            instance.transform.SetParent(_canvas.transform);
            instance.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            _cutsceneDict[_cutsceneList[i].CutsceneType] = instance;
        }
    }

    public void PlayCutScene(CutsceneType type)
    {
        StartCoroutine(_cutsceneDict[type].CutsceneExecute());
        _currentScene = _cutsceneDict[type];
    }

    public void CurrentSceneClear()
    {
        if (_currentScene != null)
        {
            _currentScene.CutsceneClear();
        }
        _currentScene = null;
    }
}
