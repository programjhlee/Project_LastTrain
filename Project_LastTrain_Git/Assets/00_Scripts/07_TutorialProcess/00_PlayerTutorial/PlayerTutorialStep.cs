using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerTutorialStep : TutorialStep
{
    protected Player _p;
    protected PlayerAction _pAction;
    public virtual void Bind(Player p)
    {
        _p = p;
        _pAction = _p.GetComponent<PlayerAction>();

    }
}
