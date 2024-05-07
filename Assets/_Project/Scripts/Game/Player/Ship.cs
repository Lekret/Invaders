using System;
using System.Collections.Generic;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Player.Weapon;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Services;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IUpdatable, IDisposable, IInputListener
    {
        private readonly Bounds _shipMovementBounds;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Vector3ReactiveProperty _position = new();
        private readonly ReactiveCommand<Ship> _destroyedCommand = new();
        private readonly ReactiveCommand<int> _healthChangedCommand = new();
        private bool _inputAttackRequested;
        private float _inputMovementDelta;
        private float _speed;
        private int _health;
        private IShipWeapon _weapon;
        
        public Ship(Bounds shipMovementBounds)
        {
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

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public void SetWeapon(IShipWeapon weapon)
        {
            _weapon = weapon;
        }
        
        public void Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _destroyedCommand.Dispose();
            _healthChangedCommand.Dispose();
        }
        
        void IInputListener.OnAttackRequested()
        {
            _inputAttackRequested = true;
        }

        void IInputListener.SetMovementDelta(float movementDelta)
        {
            _inputMovementDelta = Mathf.Clamp(movementDelta, -1f, 1f);
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            Move(deltaTime);
            HandleAttackInput(deltaTime);
        }

        private void Move(float deltaTime)
        {
            var position = _position.Value;
            position.x += _inputMovementDelta * _speed * deltaTime;
            position.x = Mathf.Clamp(position.x, _shipMovementBounds.min.x, _shipMovementBounds.max.x);
            _position.Value = position;
        }

        private void HandleAttackInput(float deltaTime)
        {
            _weapon.Update(deltaTime, _inputAttackRequested);
            _inputAttackRequested = false;
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