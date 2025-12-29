using UnityEngine;

namespace Game.Gameplay.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            // Zıplama gücü uygula
            StateMachine.VerticalVelocity = Mathf.Sqrt(StateMachine.JumpForce * -2f * StateMachine.Gravity);
        }

        protected override void OnUpdate()
        {
            // Havada da bakış atabiliriz
            Look();
            
            // Havada hareket (Air Control)
            Move(StateMachine.MoveSpeed);
            
            // Yerçekimi ve iniş kontrolü
            StateMachine.VerticalVelocity += StateMachine.Gravity * Time.deltaTime;

            if (StateMachine.Controller.isGrounded && StateMachine.VerticalVelocity <= 0f)
            {
                StateMachine.SwitchState(StateMachine.FreeLookState);
            }
        }

        protected override void OnFixedUpdate() { }
        protected override void OnExit() { }
    }
}