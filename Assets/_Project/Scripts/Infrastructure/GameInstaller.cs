using _Project.Scripts.Game;
using _Project.Scripts.Services;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStart>().AsSingle();
            Container.BindInitializableExecutionOrder<GameStart>(100);
            
            Container.Bind<PauseService>().AsSingle();
        }
    }
}
