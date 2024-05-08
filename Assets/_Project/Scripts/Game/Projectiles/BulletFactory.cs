using System;
using _Project.Scripts.Game.CoreLoop;
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

        public Bullet CreateBullet(Vector3 position, Vector2 velocity, Team team)
        {
            var bullet = new Bullet();
            bullet.Position = position;
            BulletView viewPrefab;
            
            switch (team)
            {
                case Team.Player:
                    viewPrefab = _playerConfig.BulletViewPrefab;
                    bullet.Team = Team.Player;
                    bullet.Velocity = velocity;
                    break;
                case Team.Invaders:
                    viewPrefab = _invadersConfig.BulletViewPrefab;
                    bullet.Team = Team.Invaders;
                    bullet.Velocity = velocity;
                    break;
                default:
                    Debug.LogError(team);
                    return null;
            }
            
            var bulletView = _instantiator.InstantiatePrefabForComponent<BulletView>(viewPrefab);
            bulletView.Init(bullet);
            bullet
                .DestroyedAsObservable()
                .Subscribe(_ => _gameLoop.Remove(bullet))
                .AddTo(bullet.Subscriptions);

            _gameLoop.Add(bullet);
            bullet.Init();
            return bullet;
        }
    }
}