using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services
{
    public class CameraService : IInitializable
    {
        private Camera _camera;

        void IInitializable.Initialize()
        {
            _camera = Camera.main;
        }
    }
}