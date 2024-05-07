using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly InvadersConfig _invadersConfig;
        private readonly BulletFactory _bulletFactory;
        private readonly GameSceneData _gameSceneData;
        private readonly GameLoop _gameLoop;

        public InvadersFleetFactory(
            IInstantiator instantiator,
            InvadersConfig invadersConfig,
            BulletFactory bulletFactory,
            GameSceneData gameSceneData,
            GameLoop gameLoop)
        {
            _instantiator = instantiator;
            _invadersConfig = invadersConfig;
            _bulletFactory = bulletFactory;
            _gameSceneData = gameSceneData;
            _gameLoop = gameLoop;
        }
        
        public InvadersFleet CreateInvadersFleet()
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
                    var invader = CreateInvader(rowIndex: y);
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

            _gameLoop.Add(invadersFleet);
            invadersFleet.Init();
            return invadersFleet;
        }
        
        private Invader CreateInvader(int rowIndex)
        {
            var invader = new Invader();
            var invaderView = _instantiator.InstantiatePrefabForComponent<InvaderView>(_invadersConfig.InvaderViewPrefab);
            var skin = _invadersConfig.GetSkinByRow(rowIndex);
            invaderView.Init(invader, skin);

            invader
                .DestroyedAsObservable()
                .Subscribe(_ =>
                {
                    _gameLoop.Remove(invader);
                    invaderView.DestroySelf();
                })
                .AddTo(invader.Subscriptions);

            _gameLoop.Add(invader);
            return invader;
        }
    }
}