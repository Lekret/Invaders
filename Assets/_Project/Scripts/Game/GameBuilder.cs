﻿using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
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
            CreateInvadersFleet(gameLoop);
        }

        private PlayerInput CreatePlayerInput(GameLoop gameLoop)
        {
            var playerInput = new PlayerInput(_messageBroker);
            
            gameLoop.Add(playerInput);
            return playerInput;
        }

        private Ship CreateShip(GameLoop gameLoop)
        {
            var ship = new Ship(_bulletFactory, _cameraProvider, _gameConfig);
            ship.Position = _gameSceneData.ShipSpawnPosition;
            ship.Speed = _playerConfig.ShipSpeed;
            ship.AttackCooldown = _playerConfig.AttackCooldown;
            
            var shipView = _instantiator.InstantiatePrefabForComponent<ShipView>(_playerConfig.ShipViewPrefab);
            shipView.Init(ship);
            
            gameLoop.Add(ship);
            return ship;
        }

        private InvadersFleet CreateInvadersFleet(GameLoop gameLoop)
        {
            var spawnPosition = _gameSceneData.InvadersFleetSpawnPosition;
            var invadersFleet = new InvadersFleet();

            for (var x = 0; x < _invadersConfig.RowCount; x++)
            {
                for (var y = 0; y < _invadersConfig.ColumnCount; y++)
                {
                    var invader = CreateInvader(gameLoop);
                    var horizontalViewportT = Mathf.Lerp(
                        1f - _gameConfig.AvailableScreenArea,
                        _gameConfig.AvailableScreenArea, 
                        (float) x / _invadersConfig.RowCount);
                    
                    var invaderPosition = _cameraProvider.Camera.ViewportToWorldPoint(new Vector3(horizontalViewportT, 0f));
                    invaderPosition.y = spawnPosition.y - y * _invadersConfig.VerticalSpacingBetweenEachInvader;
                    invaderPosition.z = 0f;
                    invader.Position = invaderPosition;
                }
            }

            gameLoop.Add(invadersFleet);
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