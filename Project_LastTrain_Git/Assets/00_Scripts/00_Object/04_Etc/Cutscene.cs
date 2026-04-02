using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class Cutscene : UI_Base
{
    public abstract CutsceneManager.CutsceneType CutsceneType { get;}

    public abstract IEnumerator CutsceneExecute();

    public abstract void CutsceneClear();
}
