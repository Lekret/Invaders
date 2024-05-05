using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IUpdatable, IInputListener
    {
        private readonly BulletFactory _bulletFactory;
        private readonly CameraProvider _cameraProvider;
        private readonly GameConfig _gameConfig;
        private readonly Vector3ReactiveProperty _position = new();
        private bool _wantsAttackInput;
        private float _movementDeltaInput;
        private float _attackCooldown;
        private float _speed;
        
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

        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
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
            if (_attackCooldown > 0f)
                _attackCooldown -= deltaTime;
        }

        private void HandleAttackInput()
        {
            if (_wantsAttackInput && _attackCooldown <= 0f)
            {
                _attackCooldown = 5f;
                var bullet = _bulletFactory.CreateBullet();
                bullet.Position = _position.Value;
                bullet.SetMoveDirection(Vector3.up);
            }
            
            _wantsAttackInput = false;
        }
    }
}