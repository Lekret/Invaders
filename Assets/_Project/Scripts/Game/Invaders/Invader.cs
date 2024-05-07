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
        private readonly ReactiveCommand<Invader> _destroyedCommand = new();
        private readonly ReactiveCommand<Invader> _movedCommand = new();

        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public IObservable<Vector3> PositionAsObservable() => _position;

        public IObservable<Invader> DestroyedAsObservable() => _destroyedCommand;
        
        public IObservable<Invader> MovedAsObservable() => _movedCommand;

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _destroyedCommand.Dispose();
            _movedCommand.Dispose();
        }

        public void ApplyDamage()
        {
            _destroyedCommand.Execute(this);
        }

        public void OnMoved()
        {
            _movedCommand.Execute(this);
        }
    }
}