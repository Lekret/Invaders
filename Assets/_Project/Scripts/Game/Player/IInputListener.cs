namespace _Project.Scripts.Game.Player
{
    public interface IInputListener
    {
        void OnAttackRequested();
        void SetMovementDelta(float movementDelta);
    }
}