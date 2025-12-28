using UnityEngine;

namespace Game.Gameplay.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        // Event aboneliği için input değerini burada cache'liyoruz
        private Vector2 _currentMovementInput;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            Debug.Log("Entered State: FreeLook");
            
            // Eventlere Abone Ol
            StateMachine.InputReader.MoveEvent += OnMove;
        }

        protected override void OnExit()
        {
            // Eventlerden Çık (Çok Önemli!)
            StateMachine.InputReader.MoveEvent -= OnMove;
        }

        protected override void OnUpdate()
        {
            ApplyGravity();
            HandleMovement();
        }

        protected override void OnFixedUpdate()
        {
            // Fizik işlemleri gerekirse buraya
        }

        private void OnMove(Vector2 input)
        {
            _currentMovementInput = input;
        }

        private void HandleMovement()
        {
            if (_currentMovementInput == Vector2.zero) return;

            // Kamera yönüne göre hareket
            Vector3 forward = StateMachine.MainCameraTransform.forward;
            Vector3 right = StateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * _currentMovementInput.y + right * _currentMovementInput.x).normalized;

            // Hareket
            StateMachine.Controller.Move(moveDirection * StateMachine.MoveSpeed * Time.deltaTime);

            // Dönüş
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                StateMachine.transform.rotation = Quaternion.Slerp(
                    StateMachine.transform.rotation, 
                    targetRotation, 
                    StateMachine.RotationSpeed * Time.deltaTime);
            }
        }
    }
}