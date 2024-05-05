using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Projectiles
{
    public class BulletFactory
    {
        private readonly GameConfig _gameConfig;
        private readonly IInstantiator _instantiator;
        private readonly GameLoop _gameLoop;

        public BulletFactory(GameConfig gameConfig, IInstantiator instantiator, GameLoop gameLoop)
        {
            _gameConfig = gameConfig;
            _instantiator = instantiator;
            _gameLoop = gameLoop;
        }

        public Bullet CreateBullet()
        {
            var bulletView = _instantiator.InstantiatePrefabForComponent<BulletView>(_gameConfig.BulletViewPrefab);
            var bullet = new Bullet();
            bulletView.Init(bullet);
            return bullet;
        }
    }
}