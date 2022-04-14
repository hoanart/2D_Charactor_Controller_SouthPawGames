    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerActionManager : MonoBehaviour {
        public ActMovement actMovement;
        public ActJump actJump;
        
        private PlayerInput playerInput;
        
        private InputAction moveAction;
        private InputAction jumpAction;
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            
            //Move action.
            actMovement = new ActMovement();
            moveAction = playerInput.currentActionMap.FindAction("Move");
            moveAction.performed += actMovement.OnAct;
            moveAction.canceled += actMovement.OnStop;
            
            // Jump action.
            actJump = new ActJump();
            jumpAction = playerInput.currentActionMap.FindAction("Jump");
            jumpAction.performed += actJump.OnAct;
            jumpAction.canceled += actJump.OnStop;
        }
    }
