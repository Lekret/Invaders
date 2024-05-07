using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetMovement
    {
        private readonly InvadersConfig _invadersConfig;
        private readonly InvadersFleetState _state;
        private readonly Bounds _movementBounds;
        private float _timeUntilMovement;
        private int _direction = 1;

        public InvadersFleetMovement(
            InvadersConfig invadersConfig, 
            InvadersFleetState state,
            Bounds movementBounds)
        {
            _invadersConfig = invadersConfig;
            _state = state;
            _movementBounds = movementBounds;
        }

        public int Direction => _direction;

        public void Update(float deltaTime)
        {
            if (_timeUntilMovement > 0f)
            {
                _timeUntilMovement -= deltaTime;
                if (_timeUntilMovement > 0f)
                    return;
            }

            _timeUntilMovement = CalculateNextMovementInterval();
            MoveInvaders();
        }
                
        private float CalculateNextMovementInterval()
        {
            var currentToInitial = (float) _state.CurrentCount / _state.InitialCount;
            var movementInterval = Mathf.Lerp(
                _invadersConfig.MinMovementInterval,
                _invadersConfig.MaxMovementInterval,
                currentToInitial);
            return movementInterval;
        }

        private void MoveInvaders()
        {
            var reachedSide = IsAnyReachedSide();
            if (reachedSide)
            {
                _direction *= -1;
            }

            var movementX = _invadersConfig.HorizontalMovementPerTick * _direction;
            var movementY = reachedSide ? -_invadersConfig.VerticalMovementPerReachedSide : 0f;
            var movementVec = new Vector3(movementX, movementY);

            foreach (var row in _state.Rows)
            {
                foreach (var invader in row)
                {
                    invader.Position += movementVec;
                }
            }
        }

        private bool IsAnyReachedSide()
        {
            var minX = float.MaxValue;
            var maxX = float.MinValue;

            foreach (var row in _state.Rows)
            {
                foreach (var invader in row)
                {
                    var testPosition = invader.Position;
                    if (testPosition.x < minX)
                        minX = testPosition.x;

                    if (testPosition.x > maxX)
                        maxX = testPosition.x;
                }
            }

            var isReachAnySide = minX < _movementBounds.min.x ||
                                 maxX > _movementBounds.max.x;
            return isReachAnySide;
        }
    }
}