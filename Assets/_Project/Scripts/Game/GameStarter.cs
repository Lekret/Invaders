using System;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class GameStarter : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private readonly IMessageBroker _messageBroker;
        private readonly GameBuilder _gameBuilder;
        private GameLoop _gameLoop;

        public GameStarter(IMessageBroker messageBroker, GameBuilder gameBuilder)
        {
            _messageBroker = messageBroker;
            _gameBuilder = gameBuilder;
        }

        void IInitializable.Initialize()
        {
            _messageBroker.ShowWindow<HudWindow>();
            
            _gameLoop = new GameLoop();
            _gameLoop.RegisterGameLoopDefaultOrder();
            
            _gameBuilder.CreateGame(_gameLoop);
            
            _gameLoop.Awake();
            _gameLoop.Start();
            
            Debug.Log("Game started");
        }
        
        public void Tick()
        {
            _gameLoop.Update(Time.deltaTime);
        }

        public void FixedTick()
        {
            _gameLoop.FixedUpdate(Time.deltaTime);
        }

        void IDisposable.Dispose()
        {
            _gameLoop.Dispose();
        }
    }
}