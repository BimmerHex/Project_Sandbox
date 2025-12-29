// Assets/_Game/Scripts/UI/PauseMenuController.cs
using UnityEngine;
using UnityEngine.UI;
using Game.Systems;

namespace Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pauseMenuPanel;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _quitButton;

        [Header("Configuration")]
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        private void Awake()
        {
            _pauseMenuPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPauseToggled += HandlePauseToggled;
            }

            _resumeButton.onClick.AddListener(OnResumeClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPauseToggled -= HandlePauseToggled;
            }

            _resumeButton.onClick.RemoveListener(OnResumeClicked);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            _quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void HandlePauseToggled(bool isPaused)
        {
            _pauseMenuPanel.SetActive(isPaused);
        }

        private void OnResumeClicked()
        {
            GameManager.Instance.TogglePause(false);
        }

        private void OnMainMenuClicked()
        {
            // Önce zamanı düzelt, yoksa menüde oyun donuk kalır
            Time.timeScale = 1f;
            
            SceneLoaderUtils.LoadScene(_mainMenuSceneName);
        }

        private void OnQuitClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}