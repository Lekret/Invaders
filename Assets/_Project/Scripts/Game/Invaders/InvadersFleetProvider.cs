namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetProvider
    {
        public InvadersFleet InvadersFleet { get; private set; }

        public void Init(InvadersFleet invadersFleet)
        {
            InvadersFleet = invadersFleet;
        }
    }
}