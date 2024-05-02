using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class BootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectBootstraper>().AsSingle();
        }
    }
}