namespace _Project.Scripts.Game.Events
{
    public class PlayerScoreChangedEvent
    {
        public readonly int NewScore;

        public PlayerScoreChangedEvent(int newScore)
        {
            NewScore = newScore;
        }
    }
}