using UnityEngine;
using Game.Patterns.StateMachine;
using Game.Input;

namespace Game.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : StateMachine
    {
        [field: Header("References")]
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Transform MainCameraTransform { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 12f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 15f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField] public float JumpForce { get; private set; } = 1.5f;

        // Runtime Variables
        public float VerticalVelocity { get; set; } // Yerçekimi ve Zıplama için
        public Vector2 MovementInput { get; set; } // InputReader'dan gelen veri

        // States
        public PlayerFreeLookState FreeLookState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }

        private void Awake()
        {
            if (Controller == null) Controller = GetComponent<CharacterController>();
            if (MainCameraTransform == null) MainCameraTransform = Camera.main.transform;

            // State'leri oluştur
            FreeLookState = new PlayerFreeLookState(this);
            JumpState = new PlayerJumpState(this);
        }

        private void Start()
        {
            SwitchState(FreeLookState);
        }

        private void OnEnable()
        {
            InputReader.MoveEvent += OnMove;
        }

        private void OnDisable()
        {
            InputReader.MoveEvent -= OnMove;
        }

        private void OnMove(Vector2 input)
        {
            MovementInput = input;
        }
    }
}