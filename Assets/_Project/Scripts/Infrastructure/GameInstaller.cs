using _Project.Scripts.Game;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private InvadersConfig _invadersConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_gameConfig).AsSingle();
            Container.BindInstance(_playerConfig).AsSingle();
            Container.BindInstance(_invadersConfig).AsSingle();
            
            Container.BindInterfacesTo<GameStarter>().AsSingle();
            Container.BindInitializableExecutionOrder<GameStarter>(100);

            Container.Bind<GameBuilder>().AsSingle();
            Container.Bind<PauseService>().AsSingle();
        }
    }
}
