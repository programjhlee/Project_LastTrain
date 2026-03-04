using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GravityManager : SingletonManager<GravityManager>
{
    float gravity = 40f;
    List<IGravityAffected> gravityObjects;

    public void OnDisable()
    {
        gravityObjects.Clear();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused())
        {
            return;
        }
        for(int i = 0; i < gravityObjects.Count; i++)
        {
            if(gravityObjects[i].IsActive)
            {
                AffectGravity(gravityObjects[i]);
            }
        }
    }

    public void AddList(List<IGravityAffected> lst,IGravityAffected obj)
    {
        lst.Add(obj);
    }

    public void AddGravityObj(IGravityAffected obj)
    {
        if(gravityObjects == null)
        {
            gravityObjects = new List<IGravityAffected>();
        }
        AddList(gravityObjects, obj);
    }


    public void AffectGravity(IGravityAffected obj)
    {
        if (!obj.CollideChecker.IsLanding)
        {
            obj.YVel -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (obj.YVel < 0)
            {
                obj.YVel = 0;
                obj.TargetTransform.position = new Vector3(obj.TargetTransform.position.x, obj.CollideChecker.GetLandYPos(), 0);
            }
        }
    }
    public void ClearList()
    {
        gravityObjects.Clear();
    }
}
