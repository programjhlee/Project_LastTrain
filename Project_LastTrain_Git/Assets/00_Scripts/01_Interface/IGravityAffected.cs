using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGravityAffected
{
    float YVel { get; set; }
       
    Transform TargetTransform { get;}
    LandChecker LandChecker { get; }



}
