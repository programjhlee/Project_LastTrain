using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGravityAffected
{
    bool IsActive { get; set; }
    float YVel { get; set; }
    Transform TargetTransform { get;}
    CollideChecker CollideChecker { get; }

}
