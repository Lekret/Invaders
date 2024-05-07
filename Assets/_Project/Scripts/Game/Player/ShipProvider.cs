namespace _Project.Scripts.Game.Player
{
    public class ShipProvider
    {
        public Ship Ship { get; private set; }

        public void Init(Ship ship)
        {
            Ship = ship;
        }
    }
}