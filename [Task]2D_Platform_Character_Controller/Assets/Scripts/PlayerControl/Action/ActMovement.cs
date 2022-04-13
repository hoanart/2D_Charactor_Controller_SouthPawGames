using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActMovement : IActable {
    private Vector2 direction;
    public Vector2 Direction
    {
        get=>direction;
    } 
    public void OnAct(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        direction = inputVector;
        Debug.Log(inputVector);
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        direction = Vector2.zero;
    }
}
