using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems
{
    /// <summary>
    /// Sahne yükleme işlemleri için yardımcı araçlar.
    /// </summary>
    public static class SceneLoaderUtils
    {
        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}