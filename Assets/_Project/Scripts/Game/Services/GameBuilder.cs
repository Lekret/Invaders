using _Project.Scripts.Game.Events;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using UniRx;

namespace _Project.Scripts.Game.Services
{
    public class GameBuilder
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ShipFactory _shipFactory;
        private readonly PlayerInputFactory _playerInputFactory;
        private readonly InvadersFleetFactory _invadersFleetFactory;
        private readonly PlayerScoreCounter _playerScoreCounter;
        private readonly ShipProvider _shipProvider;
        private readonly InvadersFleetProvider _invadersFleetProvider;
        
        public GameBuilder(
            IMessagePublisher messagePublisher,
            ShipFactory shipFactory, 
            PlayerInputFactory playerInputFactory,
            InvadersFleetFactory invadersFleetFactory, 
            PlayerScoreCounter playerScoreCounter, 
            ShipProvider shipProvider, 
            InvadersFleetProvider invadersFleetProvider)
        {
            _messagePublisher = messagePublisher;
            _shipFactory = shipFactory;
            _playerInputFactory = playerInputFactory;
            _invadersFleetFactory = invadersFleetFactory;
            _playerScoreCounter = playerScoreCounter;
            _shipProvider = shipProvider;
            _invadersFleetProvider = invadersFleetProvider;
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
                .Subscribe(_ => _playerScoreCounter.AddScore(1))
                .AddTo(invadersFleet.Subscriptions);
            
            ListenGameOutcome(ship, invadersFleet);
        }

        private void ListenGameOutcome(Ship ship, InvadersFleet invadersFleet)
        {
            var gameOutcomeDisposable = Disposable.Empty;
            gameOutcomeDisposable = Observable
                .Merge(
                    invadersFleet.AllInvadersDestroyedAsObservable().Select(_ => GameOutcomeType.Win),
                    invadersFleet.ReachedPlayerAsObservable().Select(_ => GameOutcomeType.Lose),
                    ship.DiedAsObservable().Select(_ => GameOutcomeType.Lose))
                .FirstOrDefault()
                .Subscribe(type =>
                {
                    if (type != GameOutcomeType.None)
                        _messagePublisher.Publish(new GameOutcomeEvent(type));
                    
                    gameOutcomeDisposable.Dispose();
                });
        }
    }
}