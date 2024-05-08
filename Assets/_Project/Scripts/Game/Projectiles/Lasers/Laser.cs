using System;
using System.Collections.Generic;
using _Project.Scripts.Game.Invaders;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles.Lasers
{
    public class Laser : IDisposable
    {
        private readonly Vector3ReactiveProperty _position = new();
        private readonly BoolReactiveProperty _isActive = new();
        private readonly ReactiveCommand _destroyedCommand = new();
        private readonly CompositeDisposable _subscriptions = new();

        public IObservable<bool> ActiveAsObservable() => _isActive;

        public IObservable<Vector3> PositionAsObservable() => _position;
        
        public IObservable<Unit> DestroyedAsObservable() => _destroyedCommand;

        public bool IsActive
        {
            get => _isActive.Value;
            set => _isActive.Value = value;
        }
        
        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public void Init()
        {
        }
        
        public void Destroy()
        {
            _destroyedCommand.Execute();
        }

        void IDisposable.Dispose()
        {
            _isActive.Dispose();
            _destroyedCommand.Dispose();
            _position.Dispose();
        }

        public void OnHitInvader(Invader invader)
        {
            invader.ApplyDamage();
        }
    }
}