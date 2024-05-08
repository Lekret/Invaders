using UnityEngine;

namespace _Project.Scripts.Game.Events
{
    public readonly struct UiMovementInputEvent
    {
        public readonly Vector2 Delta;

        public UiMovementInputEvent(Vector2 delta)
        {
            Delta = delta;
        }
    }
}