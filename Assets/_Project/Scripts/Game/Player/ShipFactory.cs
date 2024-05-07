using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Events;
using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles;
using UniRx;
using Zenject;

namespace _Project.Scripts.Game.Player
{
    public class ShipFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IMessagePublisher _messagePublisher;
        private readonly PlayerConfig _playerConfig;
        private readonly BulletFactory _bulletFactory;
        private readonly GameSceneData _gameSceneData;
        private readonly GameLoop _gameLoop;

        public ShipFactory(
            IInstantiator instantiator,
            IMessageBroker messagePublisher,
            PlayerConfig playerConfig,
            BulletFactory bulletFactory,
            GameSceneData gameSceneData, 
            GameLoop gameLoop)
        {
            _instantiator = instantiator;
            _messagePublisher = messagePublisher;
            _playerConfig = playerConfig;
            _bulletFactory = bulletFactory;
            _gameSceneData = gameSceneData;
            _gameLoop = gameLoop;
        }
        
        public Ship CreateShip()
        {
            var ship = new Ship(_bulletFactory, _gameSceneData.ShipMovementBounds);
            ship.Health = _playerConfig.ShipHealth;
            ship.Position = _gameSceneData.ShipSpawnPosition;
            ship.Speed = _playerConfig.ShipSpeed;
            ship.AttackCooldown = _playerConfig.AttackCooldown;

            var shipView = _instantiator.InstantiatePrefabForComponent<ShipView>(_playerConfig.ShipViewPrefab);
            shipView.Init(ship);
            _gameLoop.Add(ship);

            ship
                .HealthAsObservable()
                .Subscribe(x => _messagePublisher.Publish(new ShipHealthChangedEvent(x)))
                .AddTo(ship.Subscriptions);

            return ship;
        }
    }
}