using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ITrainDamageEvent
{
    public event Action<float> OnDamage;
   
}
