    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(PlayerInput),typeof(Animator))]
    public class PlayerActionManager : MonoBehaviour {
        public ActMovement actMovement;
        public ActJump actJump;

        [SerializeField]
        private LayerMask groundLayer;
        
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;

        private Animator animator;
        
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

            animator = GetComponent<Animator>();
        }

        public void ActWalkingAnimation(bool bWalking)
        {
            animator.SetBool("isWalking",bWalking);
        }

        public void ActJumpAnimation(bool bGround)
        {
            animator.SetBool("isGround",bGround);
        }

        public void ActByVelocity(float movementY)
        {
            animator.SetFloat("velocityY",movementY);
        }
        
    }
