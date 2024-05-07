using _Project.Scripts.Events;
using _Project.Scripts.Game.Core;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class GameStarter : IInitializable
    {
        private readonly GameLoop _gameLoop;
        private readonly GameBuilder _gameBuilder;
        private readonly IMessagePublisher _messagePublisher;

        public GameStarter(
            GameLoop gameLoop, 
            GameBuilder gameBuilder, 
            IMessagePublisher messagePublisher)
        {
            _gameLoop = gameLoop;
            _gameBuilder = gameBuilder;
            _messagePublisher = messagePublisher;
        }

        void IInitializable.Initialize()
        {
            _gameLoop.RegisterGameLoopDefaultOrder();
            _gameBuilder.CreateGame(_gameLoop);
            _messagePublisher.ShowWindow<HudWindow>();
            Debug.Log("Game started");
        }
    }
}