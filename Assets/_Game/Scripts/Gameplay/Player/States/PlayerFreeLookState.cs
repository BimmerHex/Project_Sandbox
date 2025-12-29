using UnityEngine;

namespace Game.Gameplay.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private bool _isSprinting;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            StateMachine.InputReader.SprintEvent += OnSprintStarted;
            StateMachine.InputReader.SprintCancelledEvent += OnSprintEnded;
            StateMachine.InputReader.JumpEvent += OnJump;
        }

        protected override void OnExit()
        {
            StateMachine.InputReader.SprintEvent -= OnSprintStarted;
            StateMachine.InputReader.SprintCancelledEvent -= OnSprintEnded;
            StateMachine.InputReader.JumpEvent -= OnJump;
        }

        protected override void OnUpdate()
        {
            // FPS Bakışı
            Look();

            // Yerçekimi
            CalculateGravity();
            
            // Hareket
            float currentSpeed = _isSprinting ? StateMachine.SprintSpeed : StateMachine.MoveSpeed;
            Move(currentSpeed);
        }

        protected override void OnFixedUpdate() { }

        private void OnSprintStarted() => _isSprinting = true;
        private void OnSprintEnded() => _isSprinting = false;

        private void OnJump()
        {
            StateMachine.SwitchState(StateMachine.JumpState);
        }
    }
}