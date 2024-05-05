using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Invaders;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles;

namespace _Project.Scripts.Game
{
    public static class GameLoopExtensions
    {
        public static void RegisterGameLoopDefaultOrder(this GameLoop gameLoop)
        {
            gameLoop
                .ThenUpdate<PlayerInput>()
                .ThenUpdate<Ship>()
                .ThenUpdate<InvadersFleet>()
                .ThenUpdate<Bullet>();

            gameLoop
                .ThenFixedUpdate<Bullet>();

#if UNITY_EDITOR
            gameLoop.EditorValidateMissingDispatchTypes();
#endif
        }
    }
}