using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollideChecker : MonoBehaviour
{

    [SerializeField] float extraCheckDistance = 0.1f;
    Collider _col;

    LayerMask _groundLayer;
    LayerMask _enemyLayer;
    LayerMask _checkLayer;

    Vector3 _rayOrigin;
    Vector3 _halfExtents;

    RaycastHit _hit;

    float _footDistance; // halfHeight

    public bool IsLanding
    {
        get;
        private set;
    }
    public bool IsBlocked
    {
        get;
        private set;
    }
    public bool IsCliff
    {
        get;
        private set;
    }
    public void Awake()
    {
        _col = GetComponent<Collider>();
        _groundLayer = LayerMask.GetMask("Ground");
        _enemyLayer = LayerMask.GetMask("Enemy");
        _checkLayer = _enemyLayer | _groundLayer;
        _halfExtents = new Vector3(_col.bounds.extents.x - 0.05f, 0.05f, _col.bounds.extents.z - 0.05f);
        _footDistance = _col.bounds.extents.y + extraCheckDistance;
    }

    public bool CollideCheckBox(Vector3 dir, Vector3 extents, LayerMask layer,out RaycastHit hit)
    {
        if(Physics.BoxCast(transform.position, extents , dir, out hit , Quaternion.identity, _footDistance,layer))
        {
            return true;

        }
        return false;
    }

    public bool CollideCheckRay(Vector3 dir, LayerMask layer ,float distance, out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, dir, out hit, distance,(1<< layer)))
        {
            Debug.Log(hit.collider.gameObject.layer);
            return true;
        }
        return false;
    }


    public bool IsFrontBlockedBy(LayerMask layer)
    {
        return CollideCheckRay(transform.forward, layer, _col.bounds.extents.x + extraCheckDistance, out _hit);
    }



    public void LandCheck()
    {
        IsLanding = CollideCheckBox(Vector3.down,_halfExtents, _checkLayer,out _hit);
    }

    public float GetLandYPos()
    {
        float hitPoint = 0;
        if(CollideCheckBox(Vector3.down,_halfExtents, _checkLayer,out _hit))
        {
            hitPoint = _hit.point.y;
        }
            
        return hitPoint + _col.bounds.extents.y;
    }

    public void CliffCheck(Vector3 moveDir)
    {
        Vector3 rayDirection = new Vector3(moveDir.x , -_col.bounds.extents.y, 0);
        rayDirection.Normalize();
        IsCliff = !Physics.Raycast(transform.position, rayDirection,1.5f,_groundLayer);
        Debug.DrawRay(transform.position, rayDirection, Color.red);
    }

    public void OnDrawGizmos()
    {
        if (_col == null) return;

        Gizmos.color = IsLanding ? Color.red : Color.green;

        Gizmos.DrawWireCube(
            transform.position,
            _halfExtents * 2f
        ); 
        Vector3 checkBoundary = transform.position + (Vector3.down * (_col.bounds.extents.y + 0.1f));
        Gizmos.DrawWireCube(
            checkBoundary,
            _halfExtents * 2f
        );
    }
}
