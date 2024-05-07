using _Project.Scripts.Game.CoreLoop;
using UniRx;

namespace _Project.Scripts.Game.Player
{
    public class PlayerInputFactory
    {
        private readonly GameLoop _gameLoop;
        private readonly IMessageReceiver _messageReceiver;

        public PlayerInputFactory(
            GameLoop gameLoop, 
            IMessageReceiver messageReceiver)
        {
            _gameLoop = gameLoop;
            _messageReceiver = messageReceiver;
        }

        public PlayerInput CreatePlayerInput()
        {
            var playerInput = new PlayerInput(_messageReceiver);
            _gameLoop.Add(playerInput);
            playerInput.Init();
            return playerInput;
        }
    }
}