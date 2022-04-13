using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    private PlayerActionManager playerActionManager;
    // Start is called before the first frame update
    void Start()
    {
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        SimulateGravity();
      
        Move();
        transform.position= new Vector2(playerActionManager.actMovement.Direction * 10f*Time.deltaTime,0);
    }
}
