using UnityEngine;

namespace Game.Systems
{
    /// <summary>
    /// Oyunun genel durumunu ve üst düzey akışını yönetir.
    /// Kalıcı (Persistent) bir Singleton gibi davranır ama Bootstrap tarafından yaratılır.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Booting,
            MainMenu,
            Gameplay,
            Paused
        }

        public GameState CurrentState { get; private set; }

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
            Debug.Log("⚙️ GameManager Initialized.");
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
            Debug.Log($"🔄 Game State Changed: {CurrentState}");
            
            // İleride burada eventler tetikleyeceğiz (OnGameStateChanged vs.)
        }
    }
}