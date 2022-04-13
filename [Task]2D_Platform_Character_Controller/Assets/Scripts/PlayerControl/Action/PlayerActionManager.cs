    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerActionManager : MonoBehaviour {
        public ActMovement actMovement;
        
        private PlayerInput playerInput;
        private InputAction moveAction;
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            actMovement = new ActMovement();

            moveAction = playerInput.currentActionMap.FindAction("Move");
            moveAction.performed += actMovement.OnAct;
            moveAction.canceled += actMovement.OnStop;
        }
    }
