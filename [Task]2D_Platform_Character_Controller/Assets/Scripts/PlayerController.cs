using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : PhysicsObject {
    [Header("Move")]
    
    [SerializeField]
    private float maxSpeed = 10.0f;

    [SerializeField]
    private float slowDown = 10.0f;
    [SerializeField]
    private float speedClamp = 15.0f;

    private bool mbWalking;
    private bool mbJump;
    [Header("Jump")]
    [SerializeField]
    private float height = 10;
    
    private SpriteRenderer spriteRenderer;

    private PlayerActionManager playerActionManager;
    
    protected float verticalSpeed;

    protected float horizontalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }
    

    void Update()
    {
        
        playerActionManager.ActJumpAnimation(IsGround);
        playerActionManager.ActByVelocity(verticalSpeed);
        
        verticalSpeed = collInfo.isBottom || collInfo.isTop ? 0 : verticalSpeed;
        horizontalSpeed = collInfo.isLeft || collInfo.isRight ? 0 : horizontalSpeed;
        
        Jump();
        
        SimulateGravity(ref verticalSpeed);
        Walk();
        Move(horizontalSpeed,verticalSpeed);
        
    }
    protected override void Walk()
    {
        Vector2 move;
        move.x = playerActionManager.actMovement.Direction;
        Flip(move.x);
        if (move.x == 0)
        {
            mbWalking = false;
            movement.x=Mathf.MoveTowards(movement.x, 0, slowDown);
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, slowDown);
            playerActionManager.ActWalkingAnimation(mbWalking);
            return;
        }

        mbWalking = true;
        playerActionManager.ActWalkingAnimation(mbWalking);
       
        horizontalSpeed += move.x * maxSpeed*Time.deltaTime;
        //movement.x += move.x * maxSpeed*Time.deltaTime;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -speedClamp, speedClamp);
        
        //movement.x = Mathf.Clamp(movement.x, -speedClamp, speedClamp);
        //movement.x += horizontalSpeed*Time.deltaTime;
        //horizontalSpeed = IsSideCollision() ? 0 : horizontalSpeed;
    }

    private void Flip(float x)
    {
        bool bFlip = spriteRenderer.flipX ? (x > 0.01f) : (x < 0.01f);

        if (bFlip && x != 0)
        {
            bounds.center = new Vector3(-bounds.center.x,0,0);
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    protected override void Jump()
    {
        bool bJump = playerActionManager.actJump.CanJump;
        if (bJump&&collInfo.isBottom)
        {
            verticalSpeed = height;
            IsGround = false;
        }

    }
    
}