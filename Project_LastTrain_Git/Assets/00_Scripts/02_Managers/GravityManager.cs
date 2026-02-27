using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GravityManager : SingletonManager<GravityManager>
{
    float gravity = 40f;
    List<IGravityAffected> gravityObjects;

    public void OnEnable()
    {
        gravityObjects = new List<IGravityAffected>();
    }

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
            if(gravityObjects[i] != null)
            {
                Debug.Log(gravityObjects[i]);
                AffectGravity(gravityObjects[i]);
            }
        }
    }

    public void AddList(List<IGravityAffected> lst,IGravityAffected obj)
    {
        lst.Add(obj);
    }

    public void RemoveObj(IGravityAffected obj)
    {
        gravityObjects.Remove(obj);
    }
    public void AddGravityObj(IGravityAffected obj)
    {
        AddList(gravityObjects, obj);
    }


    public void AffectGravity(IGravityAffected obj)
    {
        if (!obj.CollideChecker.IsLanding)
        {
            Debug.Log(obj.YVel);
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
}
