using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IUpdatable, IDisposable, IInputListener
    {
        private readonly BulletFactory _bulletFactory;
        private readonly CameraProvider _cameraProvider;
        private readonly GameConfig _gameConfig;
        private readonly Vector3ReactiveProperty _position = new();
        private readonly ReactiveCommand<Ship> _diedCommand = new();
        private bool _wantsAttackInput;
        private float _movementDeltaInput;
        private float _attackCooldown;
        private float _currentAttackCooldown;
        private float _speed;
        private int _health;
        
        public Ship(
            BulletFactory bulletFactory, 
            CameraProvider cameraProvider,
            GameConfig gameConfig)
        {
            _bulletFactory = bulletFactory;
            _cameraProvider = cameraProvider;
            _gameConfig = gameConfig;
        }

        public IObservable<Vector3> PositionAsObservable() => _position;

        public IObservable<Ship> DiedAsObservable() => _diedCommand;
        
        public int Health
        {
            get => _health;
            set => _health = value;
        }
        
        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float AttackCooldown
        {
            get => _attackCooldown;
            set => _attackCooldown = value;
        }

        public void Dispose()
        {
            _position.Dispose();
        }
        
        void IInputListener.OnAttackInput()
        {
            _wantsAttackInput = true;
        }

        void IInputListener.SetMovementDeltaInput(float movementDelta)
        {
            _movementDeltaInput = Mathf.Clamp(movementDelta, -1f, 1f);
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            Move(deltaTime);
            UpdateAttackCooldown(deltaTime);
            HandleAttackInput();
        }

        private void Move(float deltaTime)
        {
            var position = _position.Value;
            position.x += _movementDeltaInput * _speed * deltaTime;

            var viewportPosition = _cameraProvider.Camera.WorldToViewportPoint(position);
            var clampedViewportX = Mathf.Clamp(
                viewportPosition.x, 
                1f - _gameConfig.AvailableScreenArea,
                _gameConfig.AvailableScreenArea);

            if (!Mathf.Approximately(viewportPosition.x, clampedViewportX))
            {
                viewportPosition.x = clampedViewportX;
                position = _cameraProvider.Camera.ViewportToWorldPoint(viewportPosition);
            }
            
            _position.Value = position;
        }

        private void UpdateAttackCooldown(float deltaTime)
        {
            if (_currentAttackCooldown > 0f)
                _currentAttackCooldown -= deltaTime;
        }

        private void HandleAttackInput()
        {
            if (_wantsAttackInput && _currentAttackCooldown <= 0f)
            {
                _currentAttackCooldown = _attackCooldown;
                var bullet = _bulletFactory.CreateBullet(BulletType.PlayerBullet);
                bullet.Team = Team.Player;
                bullet.Position = _position.Value;
                bullet.Velocity = Vector2.up * 5f;
            }
            
            _wantsAttackInput = false;
        }

        public void ApplyDamage()
        {
            if (_health <= 0)
                return;
            
            _health--;

            if (_health <= 0)
                _diedCommand.Execute(this);
        }
    }
}