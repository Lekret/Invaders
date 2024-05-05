using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders.View
{
    public class InvaderView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionVfxPrefab;
        
        private readonly CompositeDisposable _subscriptions = new();
        private Invader _invader;
        
        public Invader Invader => _invader;

        public void Init(Invader invader)
        {
            _invader = invader;
            invader
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(this);
        }

        public void DestroySelf()
        {
            var vfx = Instantiate(_explosionVfxPrefab);
            vfx.transform.position = transform.position;
            var main = vfx.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            
            Destroy(gameObject);
        }
    }
}