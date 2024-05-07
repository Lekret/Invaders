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

        public GameBuilder(
            IMessagePublisher messagePublisher,
            ShipFactory shipFactory, 
            PlayerInputFactory playerInputFactory,
            InvadersFleetFactory invadersFleetFactory)
        {
            _messagePublisher = messagePublisher;
            _shipFactory = shipFactory;
            _playerInputFactory = playerInputFactory;
            _invadersFleetFactory = invadersFleetFactory;
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
                .Subscribe(_ => _messagePublisher.Publish(new PlayerScoreChangedEvent(invadersFleet.DestroyedInvadersCount)))
                .AddTo(invadersFleet.Subscriptions);
            
            ListenGameOutcome(ship, invadersFleet);
        }

        private void ListenGameOutcome(Ship ship, InvadersFleet invadersFleet)
        {
            var gameOutcomeDisposable = Disposable.Empty;
            gameOutcomeDisposable = Observable
                .Merge(
                    invadersFleet.AllInvadersDestroyedAsObservable().Select(_ => true),
                    invadersFleet.ReachedPlayerAsObservable().Select(_ => false),
                    ship.DiedAsObservable().Select(_ => false))
                .First()
                .Subscribe(isWin =>
                {
                    if (isWin)
                        _messagePublisher.Publish(new WinEvent());
                    else
                        _messagePublisher.Publish(new LoseEvent());
                    
                    gameOutcomeDisposable.Dispose();
                });
        }
    }
}