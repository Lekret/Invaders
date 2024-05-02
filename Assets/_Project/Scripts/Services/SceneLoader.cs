using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneLoader
    {
        public void ReloadScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}