using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class GameStarter : IInitializable
    {
        private readonly GameLoop _gameLoop;
        private readonly GameBuilder _gameBuilder;
        private readonly IMessageBroker _messageBroker;

        public GameStarter(
            GameLoop gameLoop, 
            GameBuilder gameBuilder, 
            IMessageBroker messageBroker)
        {
            _gameLoop = gameLoop;
            _gameBuilder = gameBuilder;
            _messageBroker = messageBroker;
        }

        void IInitializable.Initialize()
        {
            _gameLoop.RegisterGameLoopDefaultOrder();
            _gameBuilder.CreateGame(_gameLoop);
            _messageBroker.ShowWindow<HudWindow>();
            Debug.Log("Game started");
        }
    }
}