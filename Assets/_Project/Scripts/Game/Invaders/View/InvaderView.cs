using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders.View
{
    public class InvaderView : MonoBehaviour
    {    
        [SerializeField] private ParticleSystem _explosionVfxPrefab;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private readonly CompositeDisposable _subscriptions = new();
        private Invader _invader;
        private int _spriteIndex;
        private InvaderSkin _skin;
        
        public Invader Invader => _invader;

        public void Init(Invader invader, InvaderSkin skin)
        {
            _invader = invader;
            _skin = skin;
            PickNextSprite();
            
            invader
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            invader
                .MovedAsObservable()
                .Subscribe(_ => PickNextSprite())
                .AddTo(_subscriptions);
            
            invader
                .DestroyedAsObservable()
                .Subscribe(_ => DestroySelf())
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(this);
        }

        private void PickNextSprite()
        {
            _spriteRenderer.sprite = _skin.Sprites[_spriteIndex];
            _spriteIndex++;
            if (_spriteIndex >= _skin.Sprites.Length)
                _spriteIndex = 0;
        }

        private void DestroySelf()
        {
            var vfx = Instantiate(_explosionVfxPrefab);
            vfx.transform.position = transform.position;
            var main = vfx.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            
            Destroy(gameObject);
        }
    }
}