﻿using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Player.View;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles.Bullets.View
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2d;
        
        private readonly CompositeDisposable _subscriptions = new();
        private Bullet _bullet;
        
        public void Init(Bullet bullet)
        {
            _bullet = bullet;
            transform.position = bullet.Position;
            _rigidbody2d.velocity = bullet.Velocity;
            transform.up = bullet.Velocity.normalized;
            
            bullet
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            bullet
                .VelocityAsObservable()
                .Subscribe(x => _rigidbody2d.velocity = x)
                .AddTo(_subscriptions);
            
            bullet
                .DestroyedAsObservable()
                .Subscribe(_ => DestroySelf())
                .AddTo(bullet.Subscriptions);
            
            _subscriptions.AddTo(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out ShipView shipView))
            {
                _bullet.OnHitWithShip(shipView.Ship);
            }
            else if (other.gameObject.TryGetComponent(out InvaderView invaderView))
            {
                _bullet.OnHitWithInvader(invaderView.Invader);
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}