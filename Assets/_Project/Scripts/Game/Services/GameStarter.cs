using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Events;
using _Project.Scripts.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Services
{
    public class GameStarter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly GameLoop _gameLoop;
        private readonly GameBuilder _gameBuilder;
        private readonly PauseService _pauseService;
        private readonly IMessagePublisher _messagePublisher;

        public GameStarter(
            GameLoop gameLoop,
            GameBuilder gameBuilder,
            PauseService pauseService,
            IMessagePublisher messagePublisher)
        {
            _gameLoop = gameLoop;
            _gameBuilder = gameBuilder;
            _pauseService = pauseService;
            _messagePublisher = messagePublisher;
        }

        void IInitializable.Initialize()
        {
            _gameLoop.RegisterGameLoopDefaultOrder();
            _gameBuilder.CreateGame();
            _messagePublisher.ShowWindow<HudWindow>();
            _pauseService
                .PausedChangedAsObservable()
                .Subscribe(isPaused => _gameLoop.SetPaused(isPaused))
                .AddTo(_subscriptions);
            
            Debug.Log("Game started");
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}