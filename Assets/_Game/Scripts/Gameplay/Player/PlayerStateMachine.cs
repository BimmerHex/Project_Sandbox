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
        
        [Tooltip("Kameranın takip edeceği, karakterin kafası hizasındaki obje.")]
        [field: SerializeField] public Transform CameraRoot { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 10f;
        [field: SerializeField] public float Gravity { get; private set; } = -15f;
        [field: SerializeField] public float JumpForce { get; private set; } = 1.5f;

        [field: Header("Look Settings")]
        [field: SerializeField] public float LookSensitivityX { get; private set; } = 1.0f;
        [field: SerializeField] public float LookSensitivityY { get; private set; } = 1.0f;
        
        // Bu değerler Input cihazına göre değişmeli. Şimdilik Mouse varsayalım.
        // Gamepad kullanırken bu çarpanı kod içinde artırmak gerekebilir.
        [Tooltip("Mouse pixel delta değerini normalize etmek için çarpan. Genelde 0.1f - 0.01f arası.")]
        [field: SerializeField] public float MouseSensitivityMultiplier { get; private set; } = 10.0f; 

        [field: SerializeField] public float MaxLookUpAngle { get; private set; } = 80f;
        [field: SerializeField] public float MaxLookDownAngle { get; private set; } = 80f;

        // Runtime Variables
        public float VerticalVelocity { get; set; }
        public Vector2 MovementInput { get; set; }
        public Vector2 LookInput { get; set; }
        public float CameraPitch { get; set; }

        public PlayerFreeLookState FreeLookState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }

        private void Awake()
        {
            if (Controller == null) Controller = GetComponent<CharacterController>();

            FreeLookState = new PlayerFreeLookState(this);
            JumpState = new PlayerJumpState(this);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SwitchState(FreeLookState);
        }

        private void OnEnable()
        {
            InputReader.MoveEvent += OnMove;
            InputReader.LookEvent += OnLook;
        }

        private void OnDisable()
        {
            InputReader.MoveEvent -= OnMove;
            InputReader.LookEvent -= OnLook;
        }

        private void OnMove(Vector2 input) => MovementInput = input;
        private void OnLook(Vector2 input) => LookInput = input;
    }
}