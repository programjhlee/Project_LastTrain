using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    float _attackPower;
    float _fixPower;
    public float AttackPower
    {
        get
        {
            return _attackPower;
        }
        protected set
        {
            _attackPower = value;
        }
    }
    public float FixPower
    {
        get
        {
            return _fixPower;
        }
        protected set
        {
            _fixPower = value;
        }
    }

    public abstract void Init();

}
