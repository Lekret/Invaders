using _Project.Scripts.UI.Core;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField]
        private WindowsConfig _windowsConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_windowsConfig).AsSingle();
            Container.BindInterfacesTo<WindowsController>().AsSingle();
        }
    }
}