using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class CameraProvider : IInitializable
    {
        private Camera _camera;

        public Camera Camera => _camera;

        void IInitializable.Initialize()
        {
            _camera = Camera.main;
        }
    }
}