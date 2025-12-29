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
        
        protected void CalculateGravity()
        {
             if (StateMachine.Controller.isGrounded && StateMachine.VerticalVelocity < 0)
             {
                 StateMachine.VerticalVelocity = -2f;
             }
             else
             {
                 StateMachine.VerticalVelocity += StateMachine.Gravity * Time.deltaTime;
             }
        }

        /// <summary>
        /// FPS Hareket Mantığı: Karakterin kendi yönüne göre hareket eder.
        /// </summary>
        protected void Move(float speed)
        {
            Vector3 movement = Vector3.zero;

            // 1. Yatay Hareket (Local Space'e göre)
            // transform.right = Karakterin sağı
            // transform.forward = Karakterin ilerisi
            if (StateMachine.MovementInput != Vector2.zero)
            {
                movement = (StateMachine.transform.right * StateMachine.MovementInput.x + 
                            StateMachine.transform.forward * StateMachine.MovementInput.y).normalized;
                movement *= speed;
            }

            // 2. Dikey Hareket (Gravity / Jump)
            movement.y = StateMachine.VerticalVelocity;

            // 3. Hareketi Uygula
            StateMachine.Controller.Move(movement * Time.deltaTime);
        }

        /// <summary>
        /// FPS Bakış Mantığı: 
        /// Mouse X -> Karakteri (Gövdeyi) döndürür.
        /// Mouse Y -> CameraRoot'u (Kafayı) döndürür.
        /// </summary>
        protected void Look()
        {
            float deltaTime = Time.deltaTime;
            
            // MouseSensitivityMultiplier ile çarparak pixel delta'yı dengeledik.
            // Gamepad için ilerde IsGamepad kontrolü yapıp farklı çarpan kullanabiliriz.
            float mouseX = StateMachine.LookInput.x * StateMachine.LookSensitivityX * StateMachine.MouseSensitivityMultiplier * deltaTime;
            StateMachine.transform.Rotate(Vector3.up * mouseX);

            float mouseY = StateMachine.LookInput.y * StateMachine.LookSensitivityY * StateMachine.MouseSensitivityMultiplier * deltaTime;
            
            StateMachine.CameraPitch -= mouseY; 
            
            StateMachine.CameraPitch = Mathf.Clamp(StateMachine.CameraPitch, 
                -StateMachine.MaxLookUpAngle, 
                StateMachine.MaxLookDownAngle);

            StateMachine.CameraRoot.localRotation = Quaternion.Euler(StateMachine.CameraPitch, 0f, 0f);
        }
    }
}