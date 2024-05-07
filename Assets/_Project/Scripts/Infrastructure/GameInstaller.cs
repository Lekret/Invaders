using _Project.Scripts.Game;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameSceneData _gameSceneData;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private InvadersConfig _invadersConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_gameSceneData).AsSingle();
            Container.BindInstance(_playerConfig).AsSingle();
            Container.BindInstance(_invadersConfig).AsSingle();
            
            Container.BindInterfacesTo<GameStarter>().AsSingle();
            Container.BindInitializableExecutionOrder<GameStarter>(100);

            Container.Bind<GameRestarter>().AsSingle();
            
            Container.Bind<GameBuilder>().AsSingle();
            Container.Bind<PlayerScoreCounter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle();
            Container.BindInterfacesTo<GameOutcomeHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle();

            Container.Bind<PlayerInputFactory>().AsSingle();
            Container.Bind<ShipFactory>().AsSingle();
            Container.Bind<InvadersFleetFactory>().AsSingle();
            Container.Bind<BulletFactory>().AsSingle();
        }
    }
}
