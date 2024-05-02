using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
using UniRx;
using Zenject;

namespace _Project.Scripts.Game
{
    public class GameFlow : IInitializable
    {
        private readonly IMessageBroker _messageBroker;

        public GameFlow(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        void IInitializable.Initialize()
        {
            _messageBroker.ShowWindow<HudWindow>();
        }
    }
}