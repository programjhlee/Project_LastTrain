using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GravityManager : SingletonManager<GravityManager>
{
    float gravity = 40f;
    List<IGravityAffected> gravityObjects;
    List<IGravityAffected> removeObjects;


    public void Awake()
    {
        gravityObjects = new List<IGravityAffected>();
        removeObjects = new List<IGravityAffected>();
    }
    public void OnDisable()
    {
        gravityObjects.Clear();
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGamePlaying()&&!GameManager.Instance.IsTutorial())
        {
            return;
        }
        for(int i = 0; i < gravityObjects.Count; i++)
        {
            if(gravityObjects[i].IsActive)
            {
                AffectGravity(gravityObjects[i]);
            }
            else
            {
                removeObjects.Add(gravityObjects[i]);
            }
        }
        for (int i = 0; i < removeObjects.Count; i++)
        {
            gravityObjects.Remove(removeObjects[i]);
        }
        removeObjects.Clear();
    }

    public void AddList(List<IGravityAffected> lst,IGravityAffected obj)
    {
        lst.Add(obj);
    }

    public void AddGravityObj(IGravityAffected obj)
    {
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
                Debug.Log($"{obj} :ÂøÁö ¼º°ø! ÂøÁö À§Ä¡ {obj.TargetTransform.position}");
            }
        }
    }
    public void ClearList()
    {
        gravityObjects.Clear();
    }
}
