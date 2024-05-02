using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class GameStart : IInitializable
    {
        private readonly IMessageBroker _messageBroker;

        public GameStart(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        void IInitializable.Initialize()
        {
            _messageBroker.ShowWindow<HudWindow>();
            Debug.Log("Game started");
        }
    }
}