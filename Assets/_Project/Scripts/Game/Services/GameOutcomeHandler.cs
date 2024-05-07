using System;
using _Project.Scripts.Game.Events;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
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
            _messageBroker.Receive<WinEvent>().Subscribe(_ => OnWin()).AddTo(_subscriptions);
            _messageBroker.Receive<LoseEvent>().Subscribe(_ => OnLose()).AddTo(_subscriptions);
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
        }

        private void OnWin()
        {
            _messageBroker.ShowWindow<GameOutcomeWindow>(w => w.ConfigureAsWin());
        }

        private void OnLose()
        {
            _messageBroker.ShowWindow<GameOutcomeWindow>(w => w.ConfigureAsLose());
        }
    }
}