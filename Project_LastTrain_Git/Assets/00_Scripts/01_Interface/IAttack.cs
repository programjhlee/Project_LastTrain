using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack 
{
    public void Attack(IAttackable _attackable,Vector3 attackDir);
}
