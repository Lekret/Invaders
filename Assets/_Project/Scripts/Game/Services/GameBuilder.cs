using _Project.Scripts.Events;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Events;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class GameBuilder
    {
        private readonly IInstantiator _instantiator;
        private readonly IMessageBroker _messageBroker;
        private readonly GameConfig _gameConfig;
        private readonly PlayerConfig _playerConfig;
        private readonly InvadersConfig _invadersConfig;
        private readonly BulletFactory _bulletFactory;
        private readonly GameSceneData _gameSceneData;
        private readonly CameraProvider _cameraProvider;

        public GameBuilder(
            IInstantiator instantiator,
            IMessageBroker messageBroker,
            GameConfig gameConfig,
            PlayerConfig playerConfig,
            InvadersConfig invadersConfig,
            BulletFactory bulletFactory,
            GameSceneData gameSceneData,
            CameraProvider cameraProvider)
        {
            _instantiator = instantiator;
            _messageBroker = messageBroker;
            _gameConfig = gameConfig;
            _playerConfig = playerConfig;
            _invadersConfig = invadersConfig;
            _bulletFactory = bulletFactory;
            _gameSceneData = gameSceneData;
            _cameraProvider = cameraProvider;
        }

        public void CreateGame(GameLoop gameLoop)
        {
            var ship = CreateShip(gameLoop);
            var playerInput = CreatePlayerInput(gameLoop);
            playerInput.SetInputListener(ship);
            var invadersFleet = CreateInvadersFleet(gameLoop, ship);

            var gameOutcomeDisposable = Disposable.Empty;
            gameOutcomeDisposable = Observable
                .Merge(
                    invadersFleet.AllInvadersDestroyedAsObservable().Select(_ => true),
                    invadersFleet.ReachedPlayerAsObservable().Select(_ => false),
                    ship.DiedAsObservable().Select(_ => false))
                .First()
                .Subscribe(isWin =>
                {
                    if (isWin)
                        _messageBroker.Publish(new WinEvent());
                    else
                        _messageBroker.Publish(new LoseEvent());
                    
                    gameOutcomeDisposable.Dispose();
                });
        }

        private PlayerInput CreatePlayerInput(GameLoop gameLoop)
        {
            var playerInput = new PlayerInput(_messageBroker);
            gameLoop.Add(playerInput);
            playerInput.Init();
            return playerInput;
        }

        private Ship CreateShip(GameLoop gameLoop)
        {
            var ship = new Ship(_bulletFactory, _gameConfig, _gameSceneData.ShipMovementBounds);
            ship.Health = _playerConfig.ShipHealth;
            ship.Position = _gameSceneData.ShipSpawnPosition;
            ship.Speed = _playerConfig.ShipSpeed;
            ship.AttackCooldown = _playerConfig.AttackCooldown;

            var shipView = _instantiator.InstantiatePrefabForComponent<ShipView>(_playerConfig.ShipViewPrefab);
            shipView.Init(ship);
            gameLoop.Add(ship);

            ship
                .HealthAsObservable()
                .Subscribe(x => _messageBroker.Publish(new ShipHealthChangedEvent(x)))
                .AddTo(ship.Subscriptions);
            
            return ship;
        }

        private InvadersFleet CreateInvadersFleet(GameLoop gameLoop, Ship ship)
        {
            var spawnPosition = _gameSceneData.InvadersFleetSpawnPosition;
            var spawnOriginX = spawnPosition.x - _invadersConfig.CountInRow / 2f * _invadersConfig.SpawnHorizontalSpacing;
            var spawnOriginY = spawnPosition.y;

            var invadersFleet = new InvadersFleet(
                _invadersConfig, 
                _gameSceneData.InvadersMovementBounds,
                _bulletFactory);

            for (var x = 0; x < _invadersConfig.CountInRow; x++)
            {
                for (var y = 0; y < _invadersConfig.CountInColumn; y++)
                {
                    var invader = CreateInvader(gameLoop);
                    var invaderPosition = new Vector3
                    {
                        x = spawnOriginX + x * _invadersConfig.SpawnHorizontalSpacing,
                        y = spawnOriginY - y * _invadersConfig.SpawnVerticalSpacing,
                        z = 0f
                    };
                    invader.Position = invaderPosition;
                    invadersFleet.AddInvader(invader, rowIndex: y);
                }
            }

            gameLoop.Add(invadersFleet);
            invadersFleet.SetTargetShip(ship);
            invadersFleet.Init();
            return invadersFleet;
        }

        private Invader CreateInvader(GameLoop gameLoop)
        {
            var invader = new Invader();
            var invaderView = _instantiator.InstantiatePrefabForComponent<InvaderView>(_invadersConfig.InvaderViewPrefab);
            invaderView.Init(invader);

            invader
                .DestroyedAsObservable()
                .Subscribe(_ =>
                {
                    gameLoop.Remove(invader);
                    invaderView.DestroySelf();
                })
                .AddTo(invader.Subscriptions);

            gameLoop.Add(invader);
            return invader;
        }
    }
}