using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace _Project.Scripts.Game.Player.View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _damageDirector;
        [SerializeField] private Transform _spriteRoot;
        
        private readonly CompositeDisposable _subscriptions = new();
        private Ship _ship;
        private Vector3 _lastPosition;

        public Ship Ship => _ship;

        public void Init(Ship ship)
        {
            _ship = ship;
            transform.position = ship.Position;
            _lastPosition = ship.Position;
            
            ship
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            ship
                .HealthAsObservable()
                .Subscribe(_ =>
                {
                    _damageDirector.Stop();
                    _damageDirector.Play();
                })
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(gameObject);
        }

        private void LateUpdate()
        {
            var position = transform.position;
            var lastPosition = _lastPosition;
            _lastPosition = position;
            var direction = position - lastPosition;
            
            var directionNormalized = direction != Vector3.zero ? direction.normalized : Vector3.up;
            var targetRotationZ = directionNormalized.x switch
            {
                0 => 0f,
                > 0 => -10f,
                < 0 => 10f,
            };

            var targetRotation = Quaternion.Euler(0f, 0f, targetRotationZ);
            _spriteRoot.localRotation = Quaternion.Lerp(_spriteRoot.localRotation, targetRotation, Time.deltaTime * 8f);

        }
    }
}