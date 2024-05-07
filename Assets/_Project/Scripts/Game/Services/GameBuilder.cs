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

        public GameBuilder(
            IMessagePublisher messagePublisher,
            ShipFactory shipFactory, 
            PlayerInputFactory playerInputFactory,
            InvadersFleetFactory invadersFleetFactory, 
            PlayerScoreCounter playerScoreCounter)
        {
            _messagePublisher = messagePublisher;
            _shipFactory = shipFactory;
            _playerInputFactory = playerInputFactory;
            _invadersFleetFactory = invadersFleetFactory;
            _playerScoreCounter = playerScoreCounter;
        }

        public void CreateGame()
        {
            var ship = _shipFactory.CreateShip();
            var playerInput = _playerInputFactory.CreatePlayerInput();
            playerInput.SetInputListener(ship);
            
            var invadersFleet = _invadersFleetFactory.CreateInvadersFleet();
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