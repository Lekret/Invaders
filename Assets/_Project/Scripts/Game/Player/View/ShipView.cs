using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player.View
{
    public class ShipView : MonoBehaviour
    {
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

            _subscriptions.AddTo(this);
        }
    }
}