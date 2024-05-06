using System;
using System.Collections.Generic;
using _Project.Scripts.Game.Core;
using UniRx;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleet : IAwakeable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly List<Invader> _invaders = new();
        private readonly InvadersConfig _invadersConfig;

        public InvadersFleet(InvadersConfig invadersConfig)
        {
            _invadersConfig = invadersConfig;
        }

        public void AddInvader(Invader invader)
        {
            _invaders.Add(invader);
        }

        void IAwakeable.OnAwake()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(1f)).Subscribe(_ => MoveInvaders())
                .AddTo(_subscriptions);
        }

        private void MoveInvaders()
        {
            foreach (var invader in _invaders)
            {
                
            }
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}