namespace _Project.Scripts.Game.Player
{
    public interface IInputListener
    {
        void OnAttackInput();
        void SetMovementDeltaInput(float movementDelta);
    }
}