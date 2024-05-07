using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Player;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleet : IDisposable, IUpdatable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly ReactiveCommand _allInvadersDestroyedCommand = new();
        private readonly ReactiveCommand _reachedPlayerCommand = new();
        private readonly List<List<Invader>> _invadersRows = new();
        private readonly InvadersConfig _invadersConfig;
        private Ship _targetShip;
        private int _initialCount;
        private int _currentCount;
        private float _timeUntilMovement;
        private int _ticksUntilReachSide;
        private int _direction = 1;
        
        public InvadersFleet(InvadersConfig invadersConfig)
        {
            _invadersConfig = invadersConfig;
        }

        public IObservable<Unit> AllInvadersDestroyedAsObservable() => _allInvadersDestroyedCommand;

        public IObservable<Unit> ReachedPlayerAsObservable() => _reachedPlayerCommand;

        public void SetTargetShip(Ship targetShip)
        {
            _targetShip = targetShip;
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
            _initialCount = _invadersRows.Sum(x => x.Count);
            _currentCount = _initialCount;
            _ticksUntilReachSide = Mathf.FloorToInt(_invadersConfig.MovementTicksToReachSide / 2f);

            for (var rowIndex = 0; rowIndex < _invadersRows.Count; rowIndex++)
            {
                var row = _invadersRows[rowIndex];
                var rowIndexCopy = rowIndex;

                foreach (var invader in row)
                {
                    invader
                        .DestroyedAsObservable()
                        .Subscribe(inv => OnInvaderDied(inv, rowIndexCopy))
                        .AddTo(_subscriptions);
                }
            }
        }

        private void OnInvaderDied(Invader invader, int rowIndex)
        {
            var row = _invadersRows[rowIndex];
            row.Remove(invader);
            _currentCount--;
            if (_currentCount <= 0)
                _allInvadersDestroyedCommand.Execute();
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
            _allInvadersDestroyedCommand.Dispose();
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
            var currentToInitial = (float) _currentCount / _initialCount;
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
                CheckIfReachedTarget();
            }
        }

        private void CheckIfReachedTarget()
        {
            if (IsReachedTargetShip())
            {
                _reachedPlayerCommand.Execute();
            }
        }

        private bool IsReachedTargetShip()
        {
            var lastNonEmptyRow = _invadersRows.FindLast(l => l.Count > 0);
            if (lastNonEmptyRow == null)
                return false;
            
            var invaderFromLastRow = lastNonEmptyRow[0];
            var yDistToShip = invaderFromLastRow.Position.y - _targetShip.Position.y;
            var isReachedTarget = yDistToShip <= _invadersConfig.ReachedPlayerToleranceY;
            return isReachedTarget;
        }
    }
}