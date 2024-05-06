using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Player;

namespace _Project.Scripts.Game
{
    public static class GameLoopExtensions
    {
        public static void RegisterGameLoopDefaultOrder(this GameLoop gameLoop)
        {
            gameLoop
                .ThenUpdate<PlayerInput>()
                .ThenUpdate<Ship>();
                
#if UNITY_EDITOR
            gameLoop.EditorValidateMissingDispatchTypes();
#endif
        }
    }
}