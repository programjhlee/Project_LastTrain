using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Cutscene : MonoBehaviour
{
    public abstract void CutsceneEnter();

    public abstract IEnumerator CutsceneExecute();

    public abstract void CutsceneClear();
}
