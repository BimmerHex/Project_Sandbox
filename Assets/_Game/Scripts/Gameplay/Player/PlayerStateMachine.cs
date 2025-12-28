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

        [field: Header("Settings")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 15f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;

        // States
        public PlayerFreeLookState FreeLookState { get; private set; }

        private void Awake()
        {
            // Componentleri otomatik al
            if (Controller == null) Controller = GetComponent<CharacterController>();
            if (MainCameraTransform == null) MainCameraTransform = Camera.main.transform;

            // State'leri oluştur
            FreeLookState = new PlayerFreeLookState(this);
        }

        private void Start()
        {
            // Başlangıç State'ine geç
            SwitchState(FreeLookState);
        }
    }
}