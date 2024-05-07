using System.Collections.Generic;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetState
    {
        public readonly List<List<Invader>> Rows = new();
        public int InitialCount;
        public int CurrentCount;
    }
}