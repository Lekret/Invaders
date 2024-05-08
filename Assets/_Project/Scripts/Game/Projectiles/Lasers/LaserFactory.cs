using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles.Bullets;
using _Project.Scripts.Game.Projectiles.Bullets.View;
using _Project.Scripts.Game.Projectiles.Lasers.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Projectiles.Lasers
{
    public class LaserFactory
    {
        private readonly GameLoop _gameLoop;
        private readonly IInstantiator _instantiator;
        private readonly PlayerConfig _playerConfig;

        public LaserFactory(
            GameLoop gameLoop, 
            IInstantiator instantiator,
            PlayerConfig playerConfig)
        {
            _gameLoop = gameLoop;
            _instantiator = instantiator;
            _playerConfig = playerConfig;
        }

        public Laser CreateLaser()
        {
            var laser = new Laser();
            var laserView = _instantiator.InstantiatePrefabForComponent<LaserView>(_playerConfig.LaserViewPrefab);
            laserView.Init(laser);
            laser
                .DestroyedAsObservable()
                .Subscribe(_ => _gameLoop.Remove(laser))
                .AddTo(laser.Subscriptions);

            _gameLoop.Add(laser);
            laser.Init();
            return laser;
        }
    }
}