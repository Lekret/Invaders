using _Project.Scripts.Constants;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class ProjectBootstraper : IInitializable
    {
        private readonly SceneLoader _sceneLoader;

        public ProjectBootstraper(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        void IInitializable.Initialize()
        {
            Debug.Log("Boot started");
            Debug.Log("Boot in process...");
            Debug.Log("Ads, SDKs and other things are initialized, loading Game...");
            _sceneLoader.LoadScene(SceneNames.Game);
        }
    }
}