using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Projectiles
{
    public class BulletFactory
    {
        private readonly GameLoop _gameLoop;
        private readonly IInstantiator _instantiator;
        private readonly PlayerConfig _playerConfig;
        private readonly InvadersConfig _invadersConfig;

        public BulletFactory(
            GameLoop gameLoop, 
            IInstantiator instantiator,
            PlayerConfig playerConfig,
            InvadersConfig invadersConfig)
        {
            _gameLoop = gameLoop;
            _instantiator = instantiator;
            _playerConfig = playerConfig;
            _invadersConfig = invadersConfig;
        }

        public Bullet CreateBullet(BulletType bulletType)
        {
            var bullet = new Bullet();
            
            var bulletViewPrefab = GetBulletViewPrefab(bulletType);
            var bulletView = _instantiator.InstantiatePrefabForComponent<BulletView>(bulletViewPrefab);
            bulletView.Init(bullet);
            bullet
                .DestroyedAsObservable()
                .Subscribe(_ =>
                {
                    _gameLoop.Remove(bullet);
                    bulletView.DestroySelf();
                })
                .AddTo(bullet.Subscriptions);

            _gameLoop.Add(bullet);
            bullet.Init();
            return bullet;
        }

        private BulletView GetBulletViewPrefab(BulletType bulletType)
        {
            switch (bulletType)
            {
                case BulletType.PlayerBullet:
                    return _playerConfig.BulletViewPrefab;
                case BulletType.EnemyBullet:
                    return _invadersConfig.BulletViewPrefab;
                default:
                    throw new Exception($"[BulletFactory] Bullet view not found: {bulletType}");
            }
        }
    }
}