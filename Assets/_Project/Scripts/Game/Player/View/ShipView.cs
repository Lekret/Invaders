﻿using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player.View
{
    public class ShipView : MonoBehaviour
    {
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init(Ship ship)
        {
            ship
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);

            _subscriptions.AddTo(this);
        }
    }
}