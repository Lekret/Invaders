using _Project.Scripts.Game.Core;
using UniRx;

namespace _Project.Scripts.Game.Player
{
    public class PlayerInputFactory
    {
        private readonly GameLoop _gameLoop;
        private readonly IMessageBroker _messageBroker;

        public PlayerInputFactory(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;
        }

        public PlayerInput CreatePlayerInput()
        {
            var playerInput = new PlayerInput(_messageBroker);
            _gameLoop.Add(playerInput);
            playerInput.Init();
            return playerInput;
        }
    }
}