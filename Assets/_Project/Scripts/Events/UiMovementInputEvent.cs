namespace _Project.Scripts.Events
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