using System;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Invaders.View;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles.Lasers.View
{
    public class LaserView : MonoBehaviour
    {
        private readonly CompositeDisposable _subscriptions = new();
        private Laser _laser;
        
        public void Init(Laser laser)
        {
            _laser = laser;
            
            laser
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            laser
                .ActiveAsObservable()
                .Subscribe(x => gameObject.SetActive(x))
                .AddTo(_subscriptions);

            laser
                .DestroyedAsObservable()
                .Subscribe(_ => DestroySelf())
                .AddTo(_subscriptions);

            _subscriptions.AddTo(gameObject);
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out InvaderView invaderView))
            {
                _laser.OnHitInvader(invaderView.Invader);
            }
        }
    }
}