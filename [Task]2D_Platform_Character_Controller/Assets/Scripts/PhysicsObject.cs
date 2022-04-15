using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    
    public bool IsGround
    {
        get;
        set;
    }
    
    protected Vector3 movement;

    [Header("Collision")]
    [SerializeField]
    protected Bounds bounds;

    [SerializeField]
    protected LayerMask groundLayer;

    [SerializeField]
    protected int detectorCount;

    [SerializeField]
    protected float detectorRayLength;
    [SerializeField]
    [Range(0.0f, 0.3f)]
    protected float rayOffset = 0.1f;
    protected bool IsUpColl
    {
        get => mbUpColl;
    }
    protected bool IsDownColl
    {
        get => mbDownColl;
    }
    
    //Collision detector
    private Detector upDetector;

    private Detector downDetector;

    private Detector leftDetector;

    private Detector rightDetector;
    
    //Collision
    private bool mbUpColl;
    private bool mbDownColl;
    private bool mbLeftColl;
    private bool mbRightColl;

    [Header("Gravity")]
    [SerializeField]
    protected float fallClamp = -40.0f;

    private float gravity = -9.8f;

    [SerializeField]
    protected int freeColliderCount = 10;
    [SerializeField]
    protected CollisionInfo collInfo;
    
    protected float currentVerticalSpeed;

    protected float currentHorizontalSpeed;

    protected float skinWidth = 0.015f;
    

    
    public void CheckCollision()
    {
        // SetUpRayRange();
        // if (movement.x != 0)
        // {
        //     HorizontalCollision(ref movement);
        //       
        // }
        //
        // if (movement.y != 0)
        // {
        //     VerticalCollision(ref movement);  
        // }
        //
    }

    private void VerticalCollision(ref Vector3 movement)
    {
        float dirY = Mathf.Sign (movement.y);
        float rayLength = Mathf.Abs(movement.y) + skinWidth;
        if (dirY > 0)
        { 
            foreach (var origin in CalculateDetectorPosition(upDetector))
        {
            var hit = Physics2D.Raycast(origin, Vector2.up*dirY, rayLength, groundLayer);
            Debug.DrawRay(origin,Vector2.up*dirY*rayLength,Color.red);
            if (hit)
            {
                
                movement.y = (hit.distance - skinWidth) * dirY;
                rayLength = hit.distance;
                collInfo.CheckTopBottom(true,false);
            }
        }
        }
        else
        {
            foreach (var origin in CalculateDetectorPosition(downDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.up*dirY, rayLength, groundLayer);
                Debug.DrawRay(origin,Vector2.up*dirY*rayLength,Color.red);
                if (hit)
                {
                    IsGround = true;
                    movement.y = (hit.distance - skinWidth) * dirY;
                    rayLength = hit.distance;
                    collInfo.CheckTopBottom(false,true);
                }
            }
        }
    }

    private void HorizontalCollision(ref Vector3 movement)
    {
        float dirX = Mathf.Sign (movement.x);
        float rayLength = Mathf.Abs(movement.x) + skinWidth;
        if (dirX > 0)
        {
            foreach (var origin in CalculateDetectorPosition(rightDetector))
            {
                var hit = Physics2D.Raycast(origin, Vector2.right*dirX, rayLength, groundLayer);
                Debug.DrawRay(origin,Vector2.right*dirX*rayLength,Color.red);
                if (hit)
                {
               
                    movement.x = (hit.distance - skinWidth) *dirX;
                    rayLength = hit.distance;
                    collInfo.CheckSide(false,true);
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

                    movement.x = (hit.distance - skinWidth) * dirX;
                    rayLength = hit.distance;
                    collInfo.CheckSide(true,false);
                }
            }
        }


    }
    /// <summary>
    /// Detect Collision using Ray.
    /// </summary>
    private void SetUpRayRange()
    {
        Bounds newBound = new Bounds(transform.position+bounds.center, bounds.size);
        newBound.Expand(skinWidth*-2);
        upDetector = new Detector(
            new Vector2(newBound.min.x + rayOffset, newBound.max.y),
            new Vector2(newBound.max.x - rayOffset, newBound.max.y),
            Vector2.up);
        downDetector = new Detector(
            new Vector2(newBound.min.x + rayOffset, newBound.min.y),
            new Vector2(newBound.max.x - rayOffset, newBound.min.y),
            Vector2.down);
        leftDetector = new Detector(
            new Vector2(newBound.min.x, newBound.min.y + rayOffset),
            new Vector2(newBound.min.x, newBound.max.y - rayOffset),
            Vector2.left);
        rightDetector = new Detector(
            new Vector2(newBound.max.x, newBound.min.y + rayOffset),
            new Vector2(newBound.max.x, newBound.max.y - rayOffset),
            Vector2.right);
    }

    protected IEnumerable<Vector2> CalculateDetectorPosition(Detector detector)
    {
        for (int i = 0; i < detectorCount; i++)
        {
            float t = (float) i / (detectorCount - 1);
           yield return Vector2.Lerp(detector.start, detector.end, t);
        }
    }

    #region Gravity

    protected void SimulateGravity()
    {
        movement.y += gravity * Time.deltaTime;
        if (movement.y < fallClamp) movement.y = fallClamp;
    }
    #endregion
    
    #region Jump

    protected virtual void Jump()
    {
        
    }

    #endregion
    #region Move

    protected virtual void ComputeVelocity()
    {
        
    }

    /// <summary>
    /// Whether it collide with side
    /// </summary>
    /// <returns></returns>
    protected bool IsSideCollision()
    {
        if (currentHorizontalSpeed > 0 && mbRightColl ||
            currentHorizontalSpeed < 0 && mbLeftColl)
        {
            return true;
        }

        return false;
    }
    
    protected void Move()
    {
        SetUpRayRange();
        collInfo.Init();
        
        if (movement.x != 0)
        {
            HorizontalCollision(ref movement);
              
        }

        if (movement.y != 0)
        {
            VerticalCollision(ref movement);  
        }
        
     
        //CheckCollision();
        transform.position+= movement ;
        //movement.x = Mathf.Clamp(movement.x, -speedClamp, speedClamp);
    }
    #endregion
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position+bounds.center,bounds.size);
        
        SetUpRayRange();

    }
}
