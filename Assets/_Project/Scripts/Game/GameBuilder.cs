using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles;
using UniRx;
using Zenject;

namespace _Project.Scripts.Game
{
    public class GameBuilder
    {
        private readonly IInstantiator _instantiator;
        private readonly IMessageBroker _messageBroker;
        private readonly GameConfig _gameConfig;
        private readonly PlayerConfig _playerConfig;
        private readonly InvadersConfig _invadersConfig;
        private readonly BulletFactory _bulletFactory;

        public GameBuilder(
            IInstantiator instantiator,
            IMessageBroker messageBroker,
            GameConfig gameConfig,
            PlayerConfig playerConfig, 
            InvadersConfig invadersConfig)
        {
            _instantiator = instantiator;
            _messageBroker = messageBroker;
            _gameConfig = gameConfig;
            _playerConfig = playerConfig;
            _invadersConfig = invadersConfig;
        }

        public void CreateGame(GameLoop gameLoop)
        {
            var ship = CreateShip();
            var playerInput = CreatePlayerInput();
            playerInput.SetInputListener(ship);
            var invadersFleet = CreateInvadersFleet();

            gameLoop.AddItem(ship);
            gameLoop.AddItem(playerInput);
            gameLoop.AddItem(invadersFleet);
        }

        private PlayerInput CreatePlayerInput()
        {
            var playerInput = new PlayerInput(_messageBroker); // TODO
            return playerInput;
        }

        private Ship CreateShip()
        {
            var shipView = _instantiator.InstantiatePrefabForComponent<ShipView>(_playerConfig.ShipViewPrefab);
            var ship = new Ship(_bulletFactory);
            shipView.Init(ship);
            return ship;
        }

        private InvadersFleet CreateInvadersFleet()
        {
            var invadersFleet = new InvadersFleet(); // TODO
            return invadersFleet;
        }

        private Invader CreateInvader()
        {
            var invader = new Invader(); // TODO
            return invader;
        }
    }
}