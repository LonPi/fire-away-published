using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour
{
    int verticalRayCount = 3;
    float skinWidth = 0.015f;
    float verticalRaySpacing, horizontalRaySpacing;
    BoxCollider2D myCollider;
    Vector2 raycastTopLeft, raycastBottomLeft;
    int layerMask;
    public CollisionInfo collisionInfo;
    public State state;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        layerMask = 1 << LayerMask.NameToLayer("Collision");
        CalculateRaySpacing();
        collisionInfo.Reset();
        state.Reset();
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        verticalRaySpacing = (bounds.size.x - 2 * skinWidth) / (verticalRayCount - 1);
    }

    private void Update()
    {
        CalculateRaySpacing();
        CalculateRaycastOrigins();
    }

    void CalculateRaycastOrigins()
    {
        Vector2 _localScale = transform.localScale;
        Vector2 size = new Vector2(myCollider.size.x * Mathf.Abs(_localScale.x), myCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        Vector2 center = new Vector2(myCollider.offset.x * _localScale.x, myCollider.offset.y * _localScale.y);
        raycastBottomLeft = transform.position + new Vector3(center.x - size.x + skinWidth, center.y - size.y + skinWidth);
    }

    public void MoveYAxis(ref float deltaMovementY)
    {
        collisionInfo.Reset();
        HandleVerticalMovement(ref deltaMovementY);
        transform.Translate(new Vector2(0f, deltaMovementY));
    }

    void HandleVerticalMovement(ref float deltaMovementY)
    {
        state.isFalling = deltaMovementY < 0;
        float raycastDistance = Mathf.Abs(deltaMovementY) + skinWidth;
        Vector2 rayOrigin = state.isFalling ? raycastBottomLeft : raycastTopLeft;
        Vector2 direction = state.isFalling ? Vector2.down : Vector2.up;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x + i * verticalRaySpacing, rayOrigin.y);
            Debug.DrawRay(rayVector, direction * raycastDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, raycastDistance, layerMask);
            if (hit)
            {
                deltaMovementY = hit.point.y - rayVector.y;
                raycastDistance = Mathf.Abs(deltaMovementY) + skinWidth;
                if (state.isFalling)
                {
                    deltaMovementY += skinWidth;
                    collisionInfo.below = true;
                }
                else
                {
                    deltaMovementY -= skinWidth;
                    collisionInfo.above = true;
                }
            }
        }
    }

    public struct State
    {
        public bool isFalling;
        public void Reset()
        {
            isFalling = false;
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public void Reset()
        {
            above = below = false;
        }
    }
}