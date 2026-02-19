using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LandChecker : MonoBehaviour
{
    private int  groundLayer = 3;
    private int enemyLayer = 7;
    [SerializeField] float extraCheckDistance = 0.1f;
    Vector3 rayOrigin;
    Collider col;
    public bool IsLanding
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
        col = GetComponent<Collider>();
    }
    public void LandCheck()
    {
        Vector3 halfExtents = new Vector3(0.4f, 0.05f, 0.4f);
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, halfExtents, Vector3.down, out hit, Quaternion.identity, col.bounds.extents.y + 0.1f))
        {
            if (hit.collider.gameObject.layer == groundLayer || hit.collider.gameObject.layer == enemyLayer)
            {
                IsLanding = true;
            }
        }
        else
        {
            IsLanding = false;
        }
    }

    public float GetLandYPos()
    {
        Vector3 halfExtents = new Vector3(0.4f, 0.05f, 0.4f);
        RaycastHit hit;
        Vector3 hitPoint = Vector3.zero;
        if (Physics.BoxCast(transform.position, halfExtents, Vector3.down, out hit, Quaternion.identity, col.bounds.extents.y + 0.1f))
        {
            if (hit.collider.gameObject.layer == groundLayer || hit.collider.gameObject.layer == enemyLayer)
            {
                hitPoint = hit.point;
            }
        }
        return hitPoint.y + col.bounds.extents.y;
    }

    public void CliffCheck(Vector3 moveDir)
    {
        col = GetComponent<Collider>();
        rayOrigin = transform.position;
        Vector3 rayDirection = new Vector3(col.bounds.extents.x * moveDir.x, -col.bounds.extents.y, 0);
        rayDirection.Normalize();
        IsCliff = !Physics.Raycast(rayOrigin, rayDirection,groundLayer);
        Debug.DrawRay(rayOrigin, rayDirection, Color.red);
    }

    public void OnDrawGizmos()
    {
        if (col == null) return;
        Vector3 halfExtents = new Vector3(0.45f, 0.05f, 0.45f);

        Gizmos.color = IsLanding ? Color.red : Color.green;

        Gizmos.DrawWireCube(
            transform.position,
            halfExtents * 2f
        );
        Vector3 checkBoundary = transform.position + (Vector3.down * (col.bounds.extents.y + 0.1f));
        Gizmos.DrawWireCube(
            checkBoundary,
            halfExtents * 2f
        );
    }
}
