using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialStep : ScriptableObject
{
    public abstract void Bind(TutorialSystem system);
    public abstract void Release();
    public abstract IEnumerator Run();
}
