using System.Collections.Generic;
using _Project.Scripts.Game.Core;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleet : IUpdatable
    {
        private List<Invader> _invaders = new List<Invader>();

        void IUpdatable.OnUpdate(float deltaTime)
        {
            
        }
    }
}