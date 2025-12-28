using UnityEngine;
using Game.Patterns.StateMachine;

namespace Game.Gameplay.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        // Ortak yardımcı metodlar buraya yazılabilir (Örn: Yerçekimi uygulama)
        protected void ApplyGravity()
        {
             // Basit yerçekimi (State Machine içinde velocity tutmadığımız için burada 
             // basitçe aşağı itiyoruz, ileride velocity bazlı sisteme geçilebilir)
             if (!StateMachine.Controller.isGrounded)
             {
                 StateMachine.Controller.Move(new Vector3(0, StateMachine.Gravity, 0) * Time.deltaTime);
             }
        }
    }
}