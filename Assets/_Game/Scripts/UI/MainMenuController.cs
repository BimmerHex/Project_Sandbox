using UnityEngine;
using UnityEngine.UI;
using Game.Systems; // SceneLoaderUtils için

namespace Game.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;

        [Header("Settings")]
        [SerializeField] private string _gameplaySceneName = "Gameplay";

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
            _quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnPlayClicked()
        {
            Debug.Log("🎮 Starting Game...");
            // GameManager üzerinden state değiştirebiliriz veya direkt sahne yükleriz
            // Şimdilik direkt sahne yüklüyoruz:
            SceneLoaderUtils.LoadScene(_gameplaySceneName);
        }

        private void OnQuitClicked()
        {
            Debug.Log("❌ Quitting Game...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}