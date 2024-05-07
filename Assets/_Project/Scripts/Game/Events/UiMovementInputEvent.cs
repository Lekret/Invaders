namespace _Project.Scripts.Game.Events
{
    public readonly struct UiMovementInputEvent
    {
        public readonly float Delta;

        public UiMovementInputEvent(float delta)
        {
            Delta = delta;
        }
    }
}