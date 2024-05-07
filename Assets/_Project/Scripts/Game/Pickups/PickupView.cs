﻿using _Project.Scripts.Game.Player.View;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Pickups
{
    public class PickupView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2d;
        
        private readonly CompositeDisposable _subscriptions = new();
        private Pickup _pickup;
        
        public void Init(Pickup pickup)
        {
            _pickup = pickup;
            transform.position = pickup.Position;
            _rigidbody2d.velocity = pickup.Velocity;
            
            pickup
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            pickup
                .VelocityAsObservable()
                .Subscribe(x => _rigidbody2d.velocity = x)
                .AddTo(_subscriptions);

            pickup
                .DestroyedAsObservable()
                .Subscribe(_ => DestroySelf())
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out ShipView shipView))
            {
                _pickup.OnHitWithShip(shipView.Ship);
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}