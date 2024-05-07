namespace _Project.Scripts.Game.Events
{
    public readonly struct UiAttackInputEvent
    {
        public readonly bool IsPressed;

        public UiAttackInputEvent(bool isPressed)
        {
            IsPressed = isPressed;
        }
    }
}