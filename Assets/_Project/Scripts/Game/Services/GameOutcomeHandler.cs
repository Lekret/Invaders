using System;
using _Project.Scripts.Game.Events;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.UI.GameOutcome;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class GameOutcomeHandler : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly IMessageBroker _messageBroker;
        private readonly InvadersFleetProvider _invadersFleetProvider;
        private readonly InvadersFleetFactory _invadersFleetFactory;

        public GameOutcomeHandler(
            IMessageBroker messageBroker,
            InvadersFleetProvider invadersFleetProvider,
            InvadersFleetFactory invadersFleetFactory)
        {
            _messageBroker = messageBroker;
            _invadersFleetProvider = invadersFleetProvider;
            _invadersFleetFactory = invadersFleetFactory;
        }

        void IInitializable.Initialize()
        {
            _messageBroker
                .Receive<GameOutcomeEvent>()
                .Subscribe(e => OnOutcome(e.Type))
                .AddTo(_subscriptions);
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
        }

        private void OnOutcome(GameOutcomeType outcomeType)
        {
            switch (outcomeType)
            {
                case GameOutcomeType.Win:
                    _invadersFleetFactory.RefillFleetWithInvaders(_invadersFleetProvider.InvadersFleet);
                    break;
                case GameOutcomeType.Lose:
                    _messageBroker.ShowWindow<GameOverWindow>();
                    break;
                default:
                    Debug.LogError(outcomeType);
                    break;
            }
        }
    }
}