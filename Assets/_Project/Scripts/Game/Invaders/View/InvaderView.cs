using System;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders.View
{
    public class InvaderView : MonoBehaviour
    {
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init(Invader invader)
        {
            invader
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(this);
        }
    }
}