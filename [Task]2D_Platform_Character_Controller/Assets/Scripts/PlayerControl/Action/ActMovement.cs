using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActMovement : IActable {
    private float direction;
    public float Direction
    {
        get=>direction;
    } 
    public void OnAct(InputAction.CallbackContext context)
    {
        float inputVector = context.ReadValue<float>();
        direction = inputVector;
     
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        direction = 0;
    }
}
