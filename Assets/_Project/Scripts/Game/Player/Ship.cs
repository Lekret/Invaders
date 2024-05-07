﻿using System;
using System.Collections.Generic;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IUpdatable, IDisposable, IInputListener
    {
        private readonly BulletFactory _bulletFactory;
        private readonly Bounds _shipMovementBounds;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Vector3ReactiveProperty _position = new();
        private readonly ReactiveCommand<Ship> _destroyedCommand = new();
        private readonly ReactiveCommand<int> _healthChangedCommand = new();
        private bool _inputWantsAttack;
        private float _inputMovementDelta;
        private float _attackCooldown;
        private float _currentAttackCooldown;
        private float _speed;
        private int _health;
        
        public Ship(
            BulletFactory bulletFactory, 
            Bounds shipMovementBounds)
        {
            _bulletFactory = bulletFactory;
            _shipMovementBounds = shipMovementBounds;
        }
        
        public IObservable<Vector3> PositionAsObservable() => _position;
        
        public IObservable<int> HealthAsObservable() => _healthChangedCommand;

        public IObservable<Ship> DiedAsObservable() => _destroyedCommand;
        
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

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public void Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _destroyedCommand.Dispose();
            _healthChangedCommand.Dispose();
        }
        
        void IInputListener.OnAttackInput()
        {
            _inputWantsAttack = true;
        }

        void IInputListener.SetMovementDelta(float movementDelta)
        {
            _inputMovementDelta = Mathf.Clamp(movementDelta, -1f, 1f);
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
            position.x += _inputMovementDelta * _speed * deltaTime;
            position.x = Mathf.Clamp(position.x, _shipMovementBounds.min.x, _shipMovementBounds.max.x);
            _position.Value = position;
        }

        private void UpdateAttackCooldown(float deltaTime)
        {
            if (_currentAttackCooldown > 0f)
                _currentAttackCooldown -= deltaTime;
        }

        private void HandleAttackInput()
        {
            if (_inputWantsAttack && _currentAttackCooldown <= 0f)
            {
                _currentAttackCooldown = _attackCooldown;
                _bulletFactory.CreateBullet(_position.Value, Team.Player);
            }
            
            _inputWantsAttack = false;
        }

        public void ApplyDamage()
        {
            if (_health <= 0)
                return;
            
            _health--;
            _healthChangedCommand.Execute(_health);
            
            if (_health <= 0)
                _destroyedCommand.Execute(this);
        }
    }
}