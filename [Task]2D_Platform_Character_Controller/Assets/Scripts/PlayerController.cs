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
    private bool mbGround;
    private bool mbJump;
    [Header("Jump")]
    [SerializeField]
    private float height = 10;
    
    private SpriteRenderer spriteRenderer;

    private PlayerActionManager playerActionManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }

    void Update()
    {
        
        playerActionManager.ActJumpAnimation(IsGround);
        playerActionManager.ActByVelocity(movement.y);
        if (collInfo.isBottom || collInfo.isTop)
        {
            movement.y = 0;
        }
        SimulateGravity();
        ComputeVelocity();
        
        Jump();
        
        Move();
        
    }
    protected override void ComputeVelocity()
    {
        
        //targetVelocity = Vector2.zero;
        Vector2 move;
        move.x = playerActionManager.actMovement.Direction;
        Flip(move.x);
        if (move.x == 0)
        {
            mbWalking = false;
            movement.x=Mathf.MoveTowards(movement.x, 0, slowDown);
            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, slowDown);
            playerActionManager.ActWalkingAnimation(mbWalking);
            return;
        }

        mbWalking = true;
        playerActionManager.ActWalkingAnimation(mbWalking);
        movement.x = 0;
        //currentHorizontalSpeed += move.x * maxSpeed*Time.deltaTime;
        movement.x += move.x * maxSpeed*Time.deltaTime;
        //currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -speedClamp, speedClamp);
        
        movement.x = Mathf.Clamp(movement.x, -speedClamp, speedClamp);
        //movement.x += currentHorizontalSpeed*Time.deltaTime;
        //currentHorizontalSpeed = IsSideCollision() ? 0 : currentHorizontalSpeed;
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
            Debug.Log("점프");
            currentVerticalSpeed = height;
            IsGround = false;
        }

    }
    
}