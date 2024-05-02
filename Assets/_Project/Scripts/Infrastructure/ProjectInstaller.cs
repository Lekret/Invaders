using _Project.Scripts.Services;
using UniRx;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<IMessageBroker>().FromInstance(MessageBroker.Default).AsSingle();
        }
    }
}
