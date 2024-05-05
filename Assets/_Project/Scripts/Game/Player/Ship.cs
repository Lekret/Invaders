using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Projectiles;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IAwakeable, IUpdatable, IInputListener
    {
        private readonly BulletFactory _bulletFactory;
        private readonly Vector3ReactiveProperty _position = new();
        private bool _wantsAttackInput;
        private float _movementDeltaInput;
        private float _attackCooldown;
        
        public Ship(BulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public IObservable<Vector3> PositionAsObservable() => _position;

        void IAwakeable.OnAwake()
        {
            // TODO Place to start position
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
            Move(_movementDeltaInput, deltaTime);
            UpdateAttackCooldown(deltaTime);
            
            if (CanAttack())
            {
                ResetAttackCooldown();
                Attack();
            }
            
            ResetWantsAttack();
        }

        private void Move(float movementDelta, float deltaTime)
        {
            var position = _position.Value;
            position.x += movementDelta * deltaTime;
            _position.Value = position;
        }

        private void UpdateAttackCooldown(float deltaTime)
        {
            if (_attackCooldown > 0f)
                _attackCooldown -= deltaTime;
        }

        private bool CanAttack()
        {
            return _wantsAttackInput && _attackCooldown <= 0f;
        }

        private void ResetWantsAttack()
        {
            _wantsAttackInput = false;
        }

        private void ResetAttackCooldown()
        {
            _attackCooldown = 5f;
        }

        private void Attack()
        {
            Debug.Log("Attack");
            var bullet = _bulletFactory.CreateBullet();
            bullet.SetPosition(_position.Value);
            bullet.SetMoveDirection(Vector3.up);
        }
    }
}