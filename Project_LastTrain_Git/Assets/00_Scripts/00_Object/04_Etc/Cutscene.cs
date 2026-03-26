using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Cutscene : UI_Base
{
    public CutsceneManager.CutsceneType CutsceneType { get; protected set; }

    public abstract IEnumerator CutsceneExecute();

    public abstract void CutsceneClear();
}
