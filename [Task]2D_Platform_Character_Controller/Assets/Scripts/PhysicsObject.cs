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
    
    protected float currentVerticalSpeed;

    protected float currentHorizontalSpeed;

    protected float skinWidth = 0.015f;
    public void CheckCollision()
    {
        SetUpRayRange();
        if (movement.x != 0)
        {
            HorizontalCollision(ref movement);
              
        }

        if (movement.y != 0)
        {
            VerticalCollision(ref movement);  
        }
        
    }

    private void VerticalCollision(ref Vector3 movement)
    {
        float rayLength = Mathf.Abs(movement.y) + skinWidth;
        foreach (var origin in CalculateDetectorPosition(downDetector))
        {
            var hit = Physics2D.Raycast(origin, downDetector.dir, rayLength, groundLayer);
            Debug.DrawRay(origin,Vector3.down*rayLength,Color.red);
            if (hit)
            {
                movement.y = (hit.distance - skinWidth) * downDetector.dir.y;
                rayLength = hit.distance;
            }
        }
        foreach (var origin in CalculateDetectorPosition(upDetector))
        {
            var hit = Physics2D.Raycast(origin, upDetector.dir, rayLength, groundLayer);
            Debug.DrawRay(origin,Vector3.up*rayLength,Color.red);
            if (hit)
            {
                movement.y = (hit.distance - skinWidth) * downDetector.dir.y;
                rayLength = hit.distance;
            }
        }
    }

    private void HorizontalCollision(ref Vector3 movement)
    {
        float dirX = Mathf.Sign (movement.x);
        float rayLength = Mathf.Abs(movement.x) + skinWidth;
        foreach (var origin in CalculateDetectorPosition(leftDetector))
        {
            var hit = Physics2D.Raycast(origin, Vector2.right*dirX, rayLength, groundLayer);
            Debug.DrawRay(origin,Vector2.right*dirX*rayLength,Color.red);
            if (hit)
            {
                movement.x = (hit.distance - skinWidth) * dirX;
                rayLength = hit.distance;
            }
        }
        foreach (var origin in CalculateDetectorPosition(rightDetector))
        {
            var hit = Physics2D.Raycast(origin, rightDetector.dir*dirX, rayLength, groundLayer);
            Debug.DrawRay(origin,rightDetector.dir*dirX*rayLength,Color.red);
            if (hit)
            {
                movement.x = (hit.distance - skinWidth) *dirX;
                rayLength = hit.distance;
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
    // private float Detect(Detector detector)
    // {
    //     return ModifyDetectorPosition(detector).Select(point =>
    //         Physics2D.Raycast(point, detector.dir, detectorRayLength, groundLayer).distance);
    // }
    //
    // private IEnumerable<Vector2> ModifyDetectorPosition(Detector detector)
    // {
    //     for (int i = 0; i < detectorCount; i++)
    //     {
    //         float t = (float) i / (detectorCount - 1);
    //         yield return Vector2.Lerp(detector.start, detector.end, t);
    //     }
    // }

    void DetectVerticalCollision()
    {
        float dirX = Mathf.Sign(movement.y);
        
    }
    #region Gravity

    protected void SimulateGravity()
    {
        movement.y += gravity * Time.deltaTime;
        // if (mbDownColl)
        // {
        //     currentVerticalSpeed = currentVerticalSpeed < 0 ? 0 : currentVerticalSpeed;
        // }
        // else
        // {
        //     currentVerticalSpeed += gravity * Time.deltaTime;
        //     currentVerticalSpeed = currentVerticalSpeed < fallClamp ? fallClamp : currentVerticalSpeed;
        // }
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
        movement.y += gravity * Time.deltaTime;
        CheckCollision();
        transform.Translate(movement);
        
        //transform.Translate(movement);



        //var pos = transform.position;
        // movement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed);
        //
        // var move = movement * Time.deltaTime;
        // var movedPosition = pos + move;
        //
        // //if it is not overlap, it is moved until it hits ground.
        // var hit = Physics2D.OverlapBox(movedPosition, bounds.size, 0, groundLayer);
        // if (!hit)
        // {
        //     transform.position += move;
        //     return;
        // }
        //
        // var positionToMove = transform.position;
        // for (int i = 1; i < freeColliderCount; i++)
        // {
        //     float t = (float) i / freeColliderCount;
        //     var posLerp = Vector2.Lerp(pos, movedPosition, t);
        //
        //     if (Physics2D.OverlapBox(posLerp, bounds.size, 0, groundLayer))
        //     {
        //         transform.position = positionToMove;
        //         Debug.Log($"positionToMOve: {transform.position}");
        //         if (i == 1)
        //         {
        //             currentVerticalSpeed = currentVerticalSpeed < 0 ? 0 : currentVerticalSpeed;
        //             var dir = transform.position - hit.transform.position;
        //             transform.position += dir.normalized * move.magnitude;
        //         }
        //
        //         return;
        //     }
        //
        //     positionToMove = posLerp;
        // }
    }
    #endregion
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position+bounds.center,bounds.size);
        
        SetUpRayRange();

    }
}
