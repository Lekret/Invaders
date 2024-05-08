using System;
using System.Collections.Generic;
using _Project.Scripts.Game.Pickups.Behaviour;
using _Project.Scripts.Game.Player;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Pickups
{
    public class Pickup : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly Vector3ReactiveProperty _position = new();
        private readonly Vector2ReactiveProperty _velocity = new();
        private readonly ReactiveCommand<Pickup> _destroyedCommand = new();
        private IPickupBehaviour _pickupBehaviour;

        public void SetPickupBehaviour(IPickupBehaviour pickupBehaviour)
        {
            _pickupBehaviour = pickupBehaviour;
        }
        
        public Vector3 Position
        {
            get => _position.Value;
            set => _position.Value = value;
        }

        public Vector2 Velocity
        {
            get => _velocity.Value;
            set => _velocity.Value = value;
        }

        public ICollection<IDisposable> Subscriptions => _subscriptions;

        public IObservable<Vector3> PositionAsObservable() => _position;

        public IObservable<Vector2> VelocityAsObservable() => _velocity;
        
        public IObservable<Pickup> DestroyedAsObservable() => _destroyedCommand;
        
        public void Init()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(5f))
                .Subscribe(_ => Destroy())
                .AddTo(_subscriptions);
        }
        
        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
            _position.Dispose();
            _velocity.Dispose();
            _destroyedCommand.Dispose();
        }

        private void Destroy()
        {
            _destroyedCommand.Execute(this);
        }

        public void OnHitWithShip()
        {
            _pickupBehaviour.Execute();
            Destroy();
        }
    }
}