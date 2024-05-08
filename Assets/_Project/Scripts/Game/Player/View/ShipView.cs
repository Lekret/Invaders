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
        
        private readonly CompositeDisposable _subscriptions = new();
        private Ship _ship;

        public Ship Ship => _ship;

        public void Init(Ship ship)
        {
            _ship = ship;
            transform.position = ship.Position;
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
    }
}