using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems
{
    /// <summary>
    /// Bu script SADECE _Boot sahnesinde çalışır.
    /// Gerekli sistemleri yükler ve ardından Menüye (veya bir sonraki sahneye) geçer.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool _loadMenuDirectly = true;
        [SerializeField] private string _menuSceneName = "MainMenu";

        private void Start()
        {
            Debug.Log("🚀 Boot Sequence Started...");
            
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            // 1. GameManager Yoksa Oluştur (Prefab'den veya kodla)
            if (GameManager.Instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
                // GameManager kendi Awake() içinde DontDestroyOnLoad yapar.
            }

            // 2. Diğer Sistemler (Audio, Input, Analytics) burada başlatılabilir.
            // ...

            Debug.Log("✅ All Systems Ready.");
            
            // 3. Sonraki Sahneye Geç
            if (_loadMenuDirectly)
            {
                LoadMenu();
            }
        }

        private void LoadMenu()
        {
            Debug.Log($"➡️ Loading Scene: {_menuSceneName}");
            SceneManager.LoadSceneAsync(_menuSceneName);
        }
    }
}