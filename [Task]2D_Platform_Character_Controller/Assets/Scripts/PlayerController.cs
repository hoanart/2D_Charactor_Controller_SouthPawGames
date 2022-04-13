using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    private SpriteRenderer spriteRenderer;
    private PlayerActionManager playerActionManager;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    // Update is called once per frame
   

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = playerActionManager.actMovement.Direction;
        Flip(move.x);
        targetVelocity = move * maxSpeed;
        currentHorizontalSpeed = targetVelocity.x;
        base.ComputeVelocity();
    }

    private void Flip(float x)
    {
        bool bFlip =spriteRenderer.flipX ? (x > 0.01f) : (x < 0.01f);

        if (bFlip)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    protected override  void Jump()
    {
        
    }
}
