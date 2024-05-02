using _Project.Scripts.Services;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
        }
    }
}
