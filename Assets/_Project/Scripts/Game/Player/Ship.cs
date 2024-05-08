using System;
using System.Collections.Generic;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders;
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
        private Vector2 _inputMovementDelta;
        private float _speed;
        private int _health;
        private IShipWeapon _defaultWeapon;
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

        public IShipWeapon DefaultWeapon
        {
            get => _defaultWeapon;
            set
            {
                _defaultWeapon = value;
                if (_weapon == null)
                    SwitchWeapon(ref _weapon, value);
            }
        }
        
        public IShipWeapon Weapon
        {
            get => _weapon;
            set => SwitchWeapon(ref _weapon, value);
        }

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public void OnHitWithInvader(Invader invader)
        {
            ApplyDamage();
            invader.ApplyDamage();
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

        void IInputListener.SetMovementDelta(Vector2 movementDelta)
        {
            _inputMovementDelta = movementDelta;
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateAttack(deltaTime);
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
        
        private void UpdateMovement(float deltaTime)
        {
            var position = _position.Value;
            position += (Vector3) _inputMovementDelta * (_speed * deltaTime);
            position.x = Mathf.Clamp(position.x, _shipMovementBounds.min.x, _shipMovementBounds.max.x);
            position.y = Mathf.Clamp(position.y, _shipMovementBounds.min.y, _shipMovementBounds.max.y);
            _position.Value = position;
        }

        private void UpdateAttack(float deltaTime)
        {
            _weapon.Update(deltaTime, _inputAttackRequested);
            if (_inputAttackRequested && _weapon.IsEmpty())
                SwitchWeapon(ref _weapon, _defaultWeapon);
            
            _inputAttackRequested = false;
        }
        
        private static void SwitchWeapon(ref IShipWeapon oldWeapon, IShipWeapon newWeapon)
        {
            if (oldWeapon != null)
                oldWeapon.OnUnequipped();
            oldWeapon = newWeapon;
            oldWeapon.OnEquipped();
        }
    }
}