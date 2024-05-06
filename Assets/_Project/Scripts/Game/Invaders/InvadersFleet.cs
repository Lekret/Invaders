using System;
using System.Collections.Generic;
using _Project.Scripts.Game.Core;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleet : IDisposable, IUpdatable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly List<List<Invader>> _invadersRows = new();
        private readonly InvadersConfig _invadersConfig;
        private int _initialCount;
        private float _timeUntilMovement;
        private int _ticksUntilReachSide;
        private int _direction = 1;

        public InvadersFleet(InvadersConfig invadersConfig)
        {
            _invadersConfig = invadersConfig;
        }

        public void AddInvader(Invader invader, int rowIndex)
        {
            var safeCounter = 0;
            while (rowIndex >= _invadersRows.Count || safeCounter++ < 100)
                _invadersRows.Add(new List<Invader>());

            _invadersRows[rowIndex].Add(invader);
        }

        public void Init()
        {
            _initialCount = CalcInvadersCount();
            _ticksUntilReachSide = Mathf.FloorToInt(_invadersConfig.MovementTicksToReachSide / 2f);

            for (var rowIndex = 0; rowIndex < _invadersRows.Count; rowIndex++)
            {
                var row = _invadersRows[rowIndex];
                var rowIndexCopy = rowIndex;
                Action<Invader> onRowInvaderDestroyed = x => _invadersRows[rowIndexCopy].Remove(x);

                foreach (var invader in row)
                {
                    invader
                        .DestroyedAsObservable()
                        .Subscribe(onRowInvaderDestroyed)
                        .AddTo(_subscriptions);
                }
            }
        }
        
        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            if (_timeUntilMovement > 0f)
                _timeUntilMovement -= deltaTime;

            if (_timeUntilMovement > 0f)
                return;

            _timeUntilMovement = CalculateNextMovementInterval();
            MoveInvaders();
        }

        private float CalculateNextMovementInterval()
        {
            var currentCount = CalcInvadersCount();
            var currentToInitial = (float) currentCount / _initialCount;
            var movementInterval = Mathf.Lerp(
                _invadersConfig.MinMovementInterval,
                _invadersConfig.MaxMovementInterval,
                currentToInitial);
            return movementInterval;
        }

        private void MoveInvaders()
        {
            _ticksUntilReachSide--;
            var reachedSide = _ticksUntilReachSide <= 0;
            var movementX = _invadersConfig.HorizontalMovementPerTick * _direction;
            var movementY = reachedSide ? -_invadersConfig.VerticalMovementPerReachSide : 0f;
            var movementVec = new Vector3(movementX, movementY);

            foreach (var row in _invadersRows)
            {
                foreach (var invader in row)
                {
                    invader.Position += movementVec;
                }
            }

            if (reachedSide)
            {
                _ticksUntilReachSide = _invadersConfig.MovementTicksToReachSide;
                _direction *= -1;
            }
        }

        private int CalcInvadersCount()
        {
            var sum = 0;
            foreach (var row in _invadersRows)
            {
                sum += row.Count;
            }

            return sum;
        }
    }
}