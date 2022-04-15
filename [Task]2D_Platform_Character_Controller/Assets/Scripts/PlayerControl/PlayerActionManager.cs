    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(PlayerInput),typeof(Animator))]
    public class PlayerActionManager : MonoBehaviour {
        public ActMovement actMovement;
        public ActJump actJump;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;

        private Animator animator;
        
        private static readonly int IsGround = Animator.StringToHash("isGround");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int VelocityY = Animator.StringToHash("velocityY");

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

        /// <summary>
        /// Active walk animation
        /// </summary>
        /// <param name="bWalking">is walking?</param>
        public void ActWalkingAnimation(bool bWalking)
        {
            animator.SetBool(IsWalking,bWalking);
        }
        /// <summary>
        /// active jump animation
        /// </summary>
        /// <param name="bGround">is ground?</param>
        public void ActJumpAnimation(bool bGround)
        {
            animator.SetBool(IsGround,bGround);
        }

        /// <summary>
        /// active falling animation by movementY
        /// </summary>
        /// <param name="movementY">movement y value</param>
        public void ActByVelocity(float movementY)
        {
            animator.SetFloat(VelocityY,movementY);
        }
        
    }
