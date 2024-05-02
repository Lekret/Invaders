using _Project.Scripts.Game;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameFlow>().AsSingle();
            Container.BindInitializableExecutionOrder<GameFlow>(100);
        }
    }
}
