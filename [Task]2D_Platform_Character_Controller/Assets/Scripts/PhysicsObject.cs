using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    
    public bool IsGround { get; set; }

    protected Vector3 movement;

    [Header("Collision")]
    
    [Tooltip("충돌경계를 설정")]
    [SerializeField]
    protected Bounds bounds;
    
    [Tooltip("발을 디딜 레이어를 설정")]
    [SerializeField]
    protected LayerMask groundLayer;
    
    [Tooltip("충돌을 감지할 레이의 개수를 설정")]
    [SerializeField]
    protected int detectorCount;
    
    [Tooltip("레이 원점 위치의 오프셋")]
    [SerializeField]
    [Range(0.0f, 0.3f)]
    protected float rayOffset = 0.1f;
    
    [Tooltip("충돌 정보")]
    [SerializeField]
    protected CollisionInfo collInfo;
    
    //Collision detector
    private Detector upDetector;

    private Detector downDetector;

    private Detector leftDetector;

    private Detector rightDetector;
    
    private float innerWidth = 0.015f;

    [Header("Gravity")]
    [Tooltip("추락속도의 최소값을 설정")]
    [SerializeField]
    protected float fallClamp = -10.0f;

    private  float gravity = -9.8f;

    #region Set up ray origins
    /// <summary>
    /// Setup ray start point and end point.
    /// </summary>
    private void SetUpRayRange()
    {
        Bounds newBound = new Bounds(transform.position + bounds.center, bounds.size);
        newBound.Expand(innerWidth * -2);
        upDetector = new Detector(
            new Vector2(newBound.min.x + rayOffset, newBound.max.y),
            new Vector2(newBound.max.x - rayOffset, newBound.max.y));
        downDetector = new Detector(
            new Vector2(newBound.min.x + rayOffset, newBound.min.y),
            new Vector2(newBound.max.x - rayOffset, newBound.min.y));
        leftDetector = new Detector(
            new Vector2(newBound.min.x, newBound.min.y + rayOffset),
            new Vector2(newBound.min.x, newBound.max.y - rayOffset));
        rightDetector = new Detector(
            new Vector2(newBound.max.x, newBound.min.y + rayOffset),
            new Vector2(newBound.max.x, newBound.max.y - rayOffset));
    }

    /// <summary>
    /// Return collection that have ray origins as many as detector count.
    /// </summary>
    /// <param name="detector">the extend to which a ray will appear </param>
    /// <returns></returns>
    private IEnumerable<Vector2> CalculateDetectorPosition(Detector detector)
    {
        for (int i = 0; i < detectorCount; i++)
        {
            float t = (float) i / (detectorCount - 1);
            yield return Vector2.Lerp(detector.start, detector.end, t);
        }
    }
    #endregion
    
    #region RayCollision
    /// <summary>
    /// Detect collision about top and bottom.
    /// </summary>
    private void VerticalCollision()
    {
        float dirY = Mathf.Sign(movement.y);
        float rayLength = Mathf.Abs(movement.y) + innerWidth;
        if (dirY > 0)
        {
            foreach (var origin in CalculateDetectorPosition(upDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.up * dirY, rayLength, groundLayer);
                Debug.DrawRay(origin, Vector2.up * dirY * rayLength, Color.red);
                if (hit)
                {
                    if (hit.collider.CompareTag("Obstacle") )
                    {
                        continue;
                    }

                    movement.y = (hit.distance - innerWidth) * dirY;
                    rayLength = hit.distance;
                    collInfo.CheckTopBottom(true, false);
                }
            }
        }
        else
        {
            foreach (var origin in CalculateDetectorPosition(downDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.up * dirY, rayLength, groundLayer);
                Debug.DrawRay(origin, Vector2.up * dirY * rayLength, Color.red);
                if (hit)
                {
                    IsGround = true;
                    movement.y = (hit.distance - innerWidth) * dirY;
                    rayLength = hit.distance;
                    collInfo.CheckTopBottom(false, true);
                }
            }
        }
    }
    /// <summary>
    /// Detect collision about both side.
    /// </summary>
    private void HorizontalCollision()
    {
        float dirX = Mathf.Sign(movement.x);
        float rayLength = Mathf.Abs(movement.x) + innerWidth;
        if (dirX > 0)
        {
            foreach (var origin in CalculateDetectorPosition(rightDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.right * dirX, rayLength, groundLayer);
                Debug.DrawRay(origin, Vector2.right * dirX * rayLength, Color.red);
                if (hit)
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                        continue;
                    }

                    movement.x = (hit.distance - innerWidth) * dirX;
                    rayLength = hit.distance;
                    collInfo.CheckSide(false, true);
                }
            }
        }

        else
        {
            foreach (var origin in CalculateDetectorPosition(leftDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.right * dirX, rayLength, groundLayer);
                Debug.DrawRay(origin, Vector2.right * dirX * rayLength, Color.red);
                if (hit)
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                        continue;
                    }

                    movement.x = (hit.distance - innerWidth) * dirX;
                    rayLength = hit.distance;
                    collInfo.CheckSide(true, false);
                }
            }
        }
    }

    
    #endregion
    
    #region Gravity

    /// <summary>
    /// Apply gravity to object
    /// </summary>
    /// <param name="verticalSpeed">It is applied by gravity</param>
    protected void SimulateGravity(ref float verticalSpeed)
    {
        if (collInfo.isBottom)
        {
            if (verticalSpeed < 0)
                verticalSpeed = 0;
        }
        else
        {
            verticalSpeed += gravity * Time.deltaTime;
            IsGround = false;
            if (verticalSpeed < fallClamp) verticalSpeed = fallClamp;
        }
    }

    #endregion

    #region Jump
    /// <summary>
    /// Describe jump behaviour.
    /// </summary>
    protected virtual void Jump()
    {
    }

    #endregion

    #region Walk
    /// <summary>
    /// Describe walk behaviour.
    /// </summary>
    protected virtual void Walk()
    {
    }

    #endregion

    #region Move

    /// <summary>
    /// Player is moved by speed is changed.
    /// </summary>
    /// <param name="hSpeed">Horizontal speed</param>
    /// <param name="vSpeed">Vertical speed</param>
    protected void Move(float hSpeed, float vSpeed)
    {
        movement.y += vSpeed;
        movement.x += hSpeed;
        movement *= Time.deltaTime;
        
        SetUpRayRange();
        
        collInfo.Init();

        if (movement.x != 0)
        {
            HorizontalCollision();
        }

        if (movement.y != 0)
        {
            VerticalCollision();
        }
        
        transform.position += movement;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);
    }
}