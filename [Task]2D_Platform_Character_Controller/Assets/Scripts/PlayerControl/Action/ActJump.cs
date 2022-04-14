using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActJump : IActable {
    private bool mbJump;

    public bool CanJump
    {
        get => mbJump;
    }
    public void OnAct(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValueAsButton();
        mbJump = inputValue;
        Debug.Log($"Jump: {mbJump}");
    }

    public void OnStop(InputAction.CallbackContext context)
    {
        mbJump = false;
    }
}
