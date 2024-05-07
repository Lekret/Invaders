using System;
using _Project.Scripts.Game.Events;
using _Project.Scripts.UI.GameOutcome;
using UniRx;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class GameOutcomeHandler : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly IMessageBroker _messageBroker;

        public GameOutcomeHandler(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
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
            _messageBroker.ShowWindow<GameOutcomeWindow>(w => w.ShowOutcome(outcomeType));
        }
    }
}