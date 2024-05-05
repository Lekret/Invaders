using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class Invader : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Vector3ReactiveProperty _position = new();
        private readonly ReactiveCommand _destroyedCommand = new();

        public ICollection<IDisposable> Subscriptions => _subscriptions;
        
        public IObservable<Vector3> PositionAsObservable() => _position;

        public IObservable<Unit> DestroyedAsObservable() => _destroyedCommand;

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _destroyedCommand.Dispose();
        }
        
        public void ApplyDamage()
        {
            _destroyedCommand.Execute();
        }
    }
}