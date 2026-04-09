using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixable 
{
    public event Action OnFixed;
    public event Action<float> OnTakeFix;
    public void TakeFix(float fixPower);
}
