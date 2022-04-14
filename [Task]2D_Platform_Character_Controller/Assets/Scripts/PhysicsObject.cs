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
    
    public void CheckCollision()
    {
        SetUpRayRange();

        mbDownColl = Detect(downDetector);
        IsGround = mbDownColl;
        //mbDownColl
        
        mbUpColl = Detect(upDetector);
        mbLeftColl = Detect(leftDetector);
        mbRightColl = Detect(rightDetector);
        
    }

    /// <summary>
    /// Detect Collision using Ray.
    /// </summary>
    private void SetUpRayRange()
    {
        Bounds newBound = new Bounds(transform.position+bounds.center, bounds.size);
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

    private bool Detect(Detector detector)
    {
        return ModifyDetectorPosition(detector).Any(point =>
            Physics2D.Raycast(point, detector.dir, detectorRayLength, groundLayer));
    }

    private IEnumerable<Vector2> ModifyDetectorPosition(Detector detector)
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
        if (mbDownColl)
        {
            currentVerticalSpeed = currentVerticalSpeed < 0 ? 0 : currentVerticalSpeed;
        }
        else
        {
            currentVerticalSpeed += gravity * Time.deltaTime;
            currentVerticalSpeed = currentVerticalSpeed < fallClamp ? fallClamp : currentVerticalSpeed;
        }
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
        var pos = transform.position;
        movement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed);
        
        var move = movement * Time.deltaTime;
        var movedPosition = pos + move;
        
        //if it is not overlap, it is moved until it hits ground.
        var hit = Physics2D.OverlapBox(movedPosition, bounds.size, 0, groundLayer);
        if (!hit)
        {
            transform.position += move;
            return;
        }
        
        var positionToMove = transform.position;
        for (int i = 1; i < freeColliderCount; i++)
        {
            float t = (float) i / freeColliderCount;
            var posLerp = Vector2.Lerp(pos, movedPosition, t);
        
            if (Physics2D.OverlapBox(posLerp, bounds.size, 0, groundLayer))
            {
                transform.position = positionToMove;
                Debug.Log($"positionToMOve: {transform.position}");
                if (i == 1)
                {
                    currentVerticalSpeed = currentVerticalSpeed < 0 ? 0 : currentVerticalSpeed;
                    var dir = transform.position - hit.transform.position;
                    transform.position += dir.normalized * move.magnitude;
                }
        
                return;
            }
        
            positionToMove = posLerp;
        }
    }
    #endregion
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position+bounds.center,bounds.size);
        
        SetUpRayRange();
        Gizmos.color = Color.blue;
        foreach (Vector2 point in ModifyDetectorPosition(upDetector))
        {
            Gizmos.DrawRay(point,upDetector.dir*detectorRayLength);
        }
        foreach (Vector2 point in ModifyDetectorPosition(downDetector))
        {
            Gizmos.DrawRay(point,downDetector.dir*detectorRayLength);
        }
        foreach (Vector2 point in ModifyDetectorPosition(leftDetector))
        {
            Gizmos.DrawRay(point,leftDetector.dir*detectorRayLength);
        }
        foreach (Vector2 point in ModifyDetectorPosition(rightDetector))
        {
            Gizmos.DrawRay(point,rightDetector.dir*detectorRayLength);
        }
    }
}
