namespace _Project.Scripts.Game.Events
{
    public readonly struct GameOutcomeEvent
    {
        public readonly GameOutcomeType Type;

        public GameOutcomeEvent(GameOutcomeType type)
        {
            Type = type;
        }
    }
}