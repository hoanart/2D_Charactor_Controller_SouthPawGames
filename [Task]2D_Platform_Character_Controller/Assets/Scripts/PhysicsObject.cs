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
    [Range(0.0f, 0.3f)]
    protected float rayOffset = 0.1f;

    
    //Collision detector
    private Detector upDetector;

    private Detector downDetector;

    private Detector leftDetector;

    private Detector rightDetector;
    

    [Header("Gravity")]
    [SerializeField]
    protected float fallClamp = -40.0f;

    protected float gravity = -9.8f;
    
    [SerializeField]
    protected CollisionInfo collInfo;
    


    protected float skinWidth = 0.015f;
    

    
    private void VerticalCollision()
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

    private void HorizontalCollision()
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
                    if (hit.collider.tag == "Obstacle")
                    {
                        Debug.Log("터치");
                    }
               
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

    protected IEnumerable<Vector2> CalculateDetectorPosition(Detector detector)
    {
        for (int i = 0; i < detectorCount; i++)
        {
            float t = (float) i / (detectorCount - 1);
           yield return Vector2.Lerp(detector.start, detector.end, t);
        }
    }

    #region Gravity

    protected void SimulateGravity(ref float verticalSpeed)
    {
        if (collInfo.isBottom)
        {
            if(verticalSpeed<0)
                verticalSpeed = 0;
        }
        else
        {
            verticalSpeed += gravity*Time.deltaTime;
            if (movement.y < fallClamp) movement.y = fallClamp;
            
        }
        
    }
    #endregion
    
    #region Jump

    protected virtual void Jump()
    {
        
    }

    #endregion

    #region Walk

    protected virtual void Walk()
    {
        
    }

    #endregion
    #region Move
    
    protected void Move(float hSpeed,float vSpeed)
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
        
     
        //CheckCollision();
        transform.position+= movement ;
        //transform.Translate(movement);
        //movement.x = Mathf.Clamp(movement.x, -speedClamp, speedClamp);
    }
    #endregion
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position+bounds.center,bounds.size);
    }
}
