using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public interface IInputListener
    {
        void OnAttackRequested();
        void SetMovementDelta(Vector2 movementDelta);
    }
}