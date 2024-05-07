using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;

namespace _Project.Scripts.Game
{
    public static class GameLoopExtensions
    {
        public static void RegisterGameLoopDefaultOrder(this GameLoop gameLoop)
        {
            gameLoop
                .ThenUpdate<PlayerInput>()
                .ThenUpdate<Ship>()
                .ThenUpdate<InvadersFleet>();
                
#if UNITY_EDITOR
            gameLoop.EditorValidateMissingDispatchTypes();
#endif
        }
    }
}