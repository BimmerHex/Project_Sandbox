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
        
        /// <summary>
        /// Yerçekimini hesaplar ama CharacterController'a uygulamaz.
        /// Uygulama işi Move metodunda yapılır.
        /// </summary>
        protected void CalculateGravity()
        {
             if (StateMachine.Controller.isGrounded && StateMachine.VerticalVelocity < 0)
             {
                 // Yerdeyken hafif negatif kuvvet uygulayarak yere yapışık kalmasını sağla
                 StateMachine.VerticalVelocity = -2f;
             }
             else
             {
                 // Havadaysa yerçekimi uygula
                 StateMachine.VerticalVelocity += StateMachine.Gravity * Time.deltaTime;
             }
        }

        /// <summary>
        /// Input verisine ve Kamera açısına göre karakteri hareket ettirir.
        /// </summary>
        protected void Move(float speed)
        {
            Vector3 movement = Vector3.zero;

            // 1. Yatay Hareket (Input + Kamera)
            if (StateMachine.MovementInput != Vector2.zero)
            {
                Vector3 forward = StateMachine.MainCameraTransform.forward;
                Vector3 right = StateMachine.MainCameraTransform.right;

                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();

                movement = (forward * StateMachine.MovementInput.y + right * StateMachine.MovementInput.x).normalized;
                movement *= speed;

                // Karakteri döndür
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                StateMachine.transform.rotation = Quaternion.Slerp(
                    StateMachine.transform.rotation, 
                    targetRotation, 
                    StateMachine.RotationSpeed * Time.deltaTime);
            }

            // 2. Dikey Hareket (Gravity / Jump)
            movement.y = StateMachine.VerticalVelocity;

            // 3. Hareketi Uygula
            StateMachine.Controller.Move(movement * Time.deltaTime);
        }
    }
}