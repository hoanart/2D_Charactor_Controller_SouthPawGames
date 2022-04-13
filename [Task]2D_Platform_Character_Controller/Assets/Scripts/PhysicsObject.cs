using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public Vector3 movement;
    
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
    
    //Collision detector
    private Detector upDetector;

    private Detector downDetector;

    private Detector leftDetector;

    private Detector rightDetector;
    
    //Collision
    private bool upColl;
    private bool downColl;
    private bool leftColl;
    private bool rightColl;

    [Header("Gravity")]
    [SerializeField]
    protected float fallClamp = -40.0f;

    private float gravity = -9.8f;
    
    [Header("Jump")]
    [SerializeField] protected float height = 10;

    [SerializeField] protected float jumpBuffer = 0.1f;
    [SerializeField] protected float earyGravityModifier = 3f;
    
    
    [Header("Move")]
    [SerializeField] protected Vector2 targetVelocity;
    [SerializeField]
    protected float maxSpeed;
    
    [SerializeField]
    protected int freeColliderCount;
    
    protected float currentVerticalSpeed;

    protected float currentHorizontalSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        SimulateGravity();
        targetVelocity = Vector2.zero;
        ComputeVelocity();
        Move();
        
    }

    public void CheckCollision()
    {
        SetUpRayRange();

        downColl = Detect(downDetector);
        
        //downColl
        
        upColl = Detect(upDetector);
        leftColl = Detect(leftDetector);
        rightColl = Detect(rightDetector);
        
    }

    /// <summary>
    /// Detect Collision using Ray.
    /// </summary>
    private void SetUpRayRange()
    {
        Bounds newBound = new Bounds(transform.position, bounds.size);
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
            Vector2.left);
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
        if (downColl)
        {
            currentVerticalSpeed = currentVerticalSpeed < 0 ? 0 : currentVerticalSpeed;
        }
        else
        {
            //Jump 이후 구현.
            //float fallSpeed =

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
        if (currentHorizontalSpeed > 0 && rightColl ||
            currentHorizontalSpeed < 0 && leftColl)
        {
            currentHorizontalSpeed = 0;
        }
    }
    protected void Move()
    {
        var pos = transform.position;
        movement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed);
        //Vector3 move = movement * Time.deltaTime;
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
    }
}
