using _Project.Scripts.Game;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Pickups;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.Weapon;
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
        [SerializeField] private PickupsConfig _pickupsConfig;
        
        public override void InstallBindings()
        {
            BindConfigs();
            BindCore();
            BindPlayer();
            BindShip();
            BindInvaders();
            BindBullets();
            BindPickups();
        }

        private void BindConfigs()
        {
            Container.BindInstance(_gameSceneData).AsSingle();
            Container.BindInstance(_playerConfig).AsSingle();
            Container.BindInstance(_invadersConfig).AsSingle();
            Container
                .BindInstance(_pickupsConfig)
                .AsSingle()
                .OnInstantiated((InjectContext ctx, PickupsConfig cfg) =>
                {
                    foreach (var behaviour in cfg.Behaviours)
                    {
                        ctx.Container.Inject(behaviour);
                    }
                });
        }

        private void BindCore()
        {
            Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle();
            
            Container.BindInterfacesTo<GameStarter>().AsSingle();
            Container.BindInitializableExecutionOrder<GameStarter>(100);
            Container.Bind<GameBuilder>().AsSingle();
            Container.Bind<GameRestarter>().AsSingle();
            Container.BindInterfacesTo<GameOutcomeHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PauseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle();
        }
        
        private void BindPlayer()
        {
            Container.Bind<PlayerScoreCounter>().AsSingle();
            Container.Bind<PlayerInputFactory>().AsSingle();
        }

        private void BindShip()
        {
            Container.Bind<ShipFactory>().AsSingle();
            Container.Bind<ShipProvider>().AsSingle();
            Container.Bind<ShipWeaponFactory>().AsSingle();
        }

        private void BindInvaders()
        {
            Container.Bind<InvadersFleetFactory>().AsSingle();
            Container.Bind<InvadersFleetProvider>().AsSingle();
        }

        private void BindBullets()
        {
            Container.Bind<BulletFactory>().AsSingle();
        }
        
        private void BindPickups()
        {
            Container.Bind<PickupFactory>().AsSingle();
        }
    }
}
