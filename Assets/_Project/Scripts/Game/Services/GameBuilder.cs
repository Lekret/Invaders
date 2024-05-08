using System;
using _Project.Scripts.Game.Events;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Pickups;
using _Project.Scripts.Game.Player;
using UniRx;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Game.Services
{
    public class GameBuilder : IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly IMessagePublisher _messagePublisher;
        private readonly ShipFactory _shipFactory;
        private readonly PlayerInputFactory _playerInputFactory;
        private readonly InvadersFleetFactory _invadersFleetFactory;
        private readonly PlayerScoreCounter _playerScoreCounter;
        private readonly ShipProvider _shipProvider;
        private readonly InvadersFleetProvider _invadersFleetProvider;
        private readonly PickupFactory _pickupFactory;
        private readonly PickupsConfig _pickupsConfig;

        public GameBuilder(
            IMessagePublisher messagePublisher,
            ShipFactory shipFactory, 
            PlayerInputFactory playerInputFactory,
            InvadersFleetFactory invadersFleetFactory, 
            PlayerScoreCounter playerScoreCounter, 
            ShipProvider shipProvider, 
            InvadersFleetProvider invadersFleetProvider, 
            PickupFactory pickupFactory, 
            PickupsConfig pickupsConfig)
        {
            _messagePublisher = messagePublisher;
            _shipFactory = shipFactory;
            _playerInputFactory = playerInputFactory;
            _invadersFleetFactory = invadersFleetFactory;
            _playerScoreCounter = playerScoreCounter;
            _shipProvider = shipProvider;
            _invadersFleetProvider = invadersFleetProvider;
            _pickupFactory = pickupFactory;
            _pickupsConfig = pickupsConfig;
        }

        public void CreateGame()
        {
            var ship = _shipFactory.CreateShip();
            _shipProvider.Init(ship);
            var playerInput = _playerInputFactory.CreatePlayerInput();
            playerInput.SetInputListener(ship);
            
            var invadersFleet = _invadersFleetFactory.CreateInvadersFleet();
            _invadersFleetProvider.Init(invadersFleet);
            invadersFleet.SetTargetShip(ship);

            invadersFleet
                .InvaderDestroyedAsObservable()
                .Subscribe(OnInvaderDied)
                .AddTo(invadersFleet.Subscriptions);
            
            ListenGameOutcome(ship, invadersFleet);
        }

        private void OnInvaderDied(Invader invader)
        {
            _playerScoreCounter.AddScore(1);
            if (_pickupsConfig.PickupSpawnProbability > Random.Range(0f, 0.999f))
                _pickupFactory.CreateRandomPickup(invader.Position);
        }

        private void ListenGameOutcome(Ship ship, InvadersFleet invadersFleet)
        {
            Observable
                .Merge(
                    invadersFleet.AllInvadersDestroyedAsObservable().Select(_ => GameOutcomeType.Win),
                    invadersFleet.ReachedPlayerAsObservable().Select(_ => GameOutcomeType.Lose),
                    ship.DiedAsObservable().Select(_ => GameOutcomeType.Lose))
                .Subscribe(type =>
                {
                    if (type != GameOutcomeType.None)
                        _messagePublisher.Publish(new GameOutcomeEvent(type));
                })
                .AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}