using _Project.Scripts.Services;

namespace _Project.Scripts.Game.Services
{
    public class GameRestarter
    {
        private readonly SceneLoader _sceneLoader;

        public GameRestarter(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void RestartGame()
        {
            _sceneLoader.ReloadScene();
        }
    }
}