using UnityEngine;

namespace Game.Gameplay.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            // Debug.Log("Entered State: Jump");
            
            // Fizik formülü: v = sqrt(h * -2 * g)
            StateMachine.VerticalVelocity = Mathf.Sqrt(StateMachine.JumpForce * -2f * StateMachine.Gravity);
        }

        protected override void OnUpdate()
        {
            // Havada hareket etmeye izin veriyoruz (Air Control)
            // İstenirse hızı StateMachine.MoveSpeed ile sınırlayabiliriz (Sprint yapamasın diye)
            Move(StateMachine.MoveSpeed);
            
            // Yerçekimi uygula (Move içinde uygulanacak ama hesaplanması lazım)
            StateMachine.VerticalVelocity += StateMachine.Gravity * Time.deltaTime;

            // Yere düştük mü?
            // VerticalVelocity <= 0 kontrolü, zıpladığımız an yere değiyor sayılmamamız için önemli.
            if (StateMachine.Controller.isGrounded && StateMachine.VerticalVelocity <= 0f)
            {
                StateMachine.SwitchState(StateMachine.FreeLookState);
            }
        }

        protected override void OnFixedUpdate() { }
        protected override void OnExit() { }
    }
}