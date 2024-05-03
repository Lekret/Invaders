namespace _Project.Scripts.Game.Player
{
    public interface IInputListener
    {
        void SetWantsAttack();
        void SetMovementDelta(float movementDelta);
    }
}