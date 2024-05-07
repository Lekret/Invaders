namespace _Project.Scripts.Game.Events
{
    public readonly struct ShipHealthChangedEvent
    {
        public readonly int CurrentValue;

        public ShipHealthChangedEvent(int currentValue)
        {
            CurrentValue = currentValue;
        }
    }
}