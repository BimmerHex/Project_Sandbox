// Assets/_Game/Scripts/Systems/Bootstrapper.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Input; // Eklendi

namespace Game.Systems
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool _loadMenuDirectly = true;
        [SerializeField] private string _menuSceneName = "MainMenu";
        
        [Header("Dependencies")]
        [SerializeField] private InputReader _inputReader; // Eklendi

        private void Start()
        {
            Debug.Log("🚀 Boot Sequence Started...");
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            // 1. GameManager Oluştur
            if (GameManager.Instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                GameManager gameManagerScript = gm.AddComponent<GameManager>();
                
                // Dependency Injection: InputReader'ı GameManager'a veriyoruz
                gameManagerScript.SetInputReader(_inputReader);
            }
            else
            {
                GameManager.Instance.SetInputReader(_inputReader);
            }

            Debug.Log("✅ All Systems Ready.");
            
            if (_loadMenuDirectly)
            {
                LoadMenu();
            }
        }

        private void LoadMenu()
        {
            SceneManager.LoadSceneAsync(_menuSceneName);
        }
    }
}