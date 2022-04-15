using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : PhysicsObject {
    [Header("Walk")]
    [Tooltip("최대 속도")]
    [SerializeField]
    private float maxSpeed = 10.0f;
    [Tooltip("감속 속도")]
    [SerializeField]
    private float slowDown = 10.0f;
    [Tooltip("증가되는 속도 한계점")]
    [SerializeField]
    private float speedClamp = 15.0f;

    [Header("Jump")]
    [Tooltip("점프 높이")]
    [SerializeField]
    private float height = 10;

    
    [SerializeField]
    [Tooltip("수직 속도")]
    private float verticalSpeed;
    [SerializeField]
    [Tooltip("수평 속도")]
    private float horizontalSpeed;
    
    private SpriteRenderer spriteRenderer;

    private PlayerActionManager playerActionManager;

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
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, slowDown);
            playerActionManager.ActWalkingAnimation(false);
            return;
        }
        
        horizontalSpeed += move.x * maxSpeed*Time.deltaTime;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -speedClamp, speedClamp);
        playerActionManager.ActWalkingAnimation(true);
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
    
    /// <summary>
    /// Flip the sprite.
    /// </summary>
    /// <param name="x">Input key value</param>
    private void Flip(float x)
    {
        bool bFlip = spriteRenderer.flipX ? (x > 0.01f) : (x < 0.01f);

        if (bFlip && x != 0)
        {
            bounds.center = new Vector3(-bounds.center.x,0,0);
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    
}