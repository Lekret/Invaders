using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Projectiles
{
    public class BulletFactory
    {
        private readonly GameConfig _gameConfig;
        private readonly IInstantiator _instantiator;

        public BulletFactory(GameConfig gameConfig, IInstantiator instantiator)
        {
            _gameConfig = gameConfig;
            _instantiator = instantiator;
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