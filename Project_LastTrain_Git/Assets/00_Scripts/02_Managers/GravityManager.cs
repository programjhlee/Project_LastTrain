using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : SingletonManager<GravityManager>
{
    float gravity = 40f;
    List<IGravityAffected> gravityObjects = new List<IGravityAffected>();
    public void OnDisable()
    {
        gravityObjects.Clear();
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        for(int i = 0; i < gravityObjects.Count; i++)
        {
            if(gravityObjects[i] != null)
            {
                gravityObjects[i].AffectGravity(gravity);
            }
        }
    }

    public void AddList(List<IGravityAffected> lst,IGravityAffected obj)
    {
        lst.Add(obj);
    }

    public void AddGravityObj(IGravityAffected obj)
    {
        AddList(gravityObjects, obj);
    }
}
