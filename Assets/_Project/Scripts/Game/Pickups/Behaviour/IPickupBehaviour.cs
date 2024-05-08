using UnityEngine;

namespace _Project.Scripts.Game.Pickups.Behaviour
{
    public interface IPickupBehaviour
    {
        Sprite IconSprite { get; }
        void Execute();
    }
}