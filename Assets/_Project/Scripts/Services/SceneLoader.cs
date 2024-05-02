using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneLoader
    {
        public void LoadScene(string sceneName)
        {
            Debug.Log($"[SceneLoader] Load scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadScene()
        {
            Debug.Log("[SceneLoader] Reload scene");
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}