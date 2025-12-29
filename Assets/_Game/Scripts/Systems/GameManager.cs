// Assets/_Game/Scripts/Systems/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement; 
using Game.Input;
using System;

namespace Game.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private InputReader _inputReader;

        public enum GameState
        {
            Booting,
            MainMenu,
            Gameplay,
            Paused
        }

        public GameState CurrentState { get; private set; }
        
        public event Action<bool> OnPauseToggled; 

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            CurrentState = GameState.Booting;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            if(_inputReader != null)
            {
                _inputReader.PauseEvent += HandlePauseInput;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if(_inputReader != null)
            {
                _inputReader.PauseEvent -= HandlePauseInput;
            }
        }

        public void SetInputReader(InputReader inputReader)
        {
            if (_inputReader != null) _inputReader.PauseEvent -= HandlePauseInput;
            _inputReader = inputReader;
            if (_inputReader != null) _inputReader.PauseEvent += HandlePauseInput;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "MainMenu":
                    SetState(GameState.MainMenu);
                    break;
                case "Gameplay":
                    SetState(GameState.Gameplay);
                    break;
                case "_Boot":
                    SetState(GameState.Booting);
                    break;
                default:
                    Debug.LogWarning($"⚠️ Unknown scene loaded: {scene.name}. State logic might need update.");
                    break;
            }
        }

        public void SetState(GameState newState)
        {
            // IDEMPOTENCY CHECK (Aynı durumdaysak tekrar işlem yapma)
            // Bu kontrol, gereksiz logları ve event tetiklemelerini önler.
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"🔄 Game State Changed: {CurrentState}");
            
            if (_inputReader != null)
            {
                switch (CurrentState)
                {
                    case GameState.MainMenu:
                        _inputReader.EnableUIInput();
                        Time.timeScale = 1f; 
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        break;
                    case GameState.Gameplay:
                        _inputReader.EnableGameplayInput();
                        Time.timeScale = 1f;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        break;
                }
            }
        }

        private void HandlePauseInput()
        {
            if (CurrentState == GameState.Gameplay)
            {
                TogglePause(true);
            }
            else if (CurrentState == GameState.Paused)
            {
                TogglePause(false);
            }
        }

        public void TogglePause(bool isPaused)
        {
            if (isPaused)
            {
                SetState(GameState.Paused);
                Time.timeScale = 0f; 
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                if(_inputReader != null) _inputReader.EnableUIInput();
            }
            else
            {
                SetState(GameState.Gameplay);
                Time.timeScale = 1f; 
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if(_inputReader != null) _inputReader.EnableGameplayInput();
            }

            OnPauseToggled?.Invoke(isPaused);
        }
    }
}