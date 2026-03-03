using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAttackable 
{
    float Maxhp { get; set; }
    float Curhp { get; set; }
    event Action OnDamaged;
    event Action<Enemy> OnDied;
    void TakeDamage(float damage, Vector3 dir);
}
