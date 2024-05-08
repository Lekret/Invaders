using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Projectiles.Bullets;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleet : IDisposable, IUpdatable
    {
        private readonly CompositeDisposable _invaderSubscriptions = new();
        private readonly ReactiveCommand<Invader> _invaderDestroyedCommand = new();
        private readonly ReactiveCommand _allInvadersDestroyedCommand = new();
        private readonly ReactiveCommand _reachedBottomCommand = new();
        private readonly InvadersFleetState _state = new();
        private readonly InvadersFleetMovement _invadersFleetMovement;
        private readonly InvadersFleetAttack _invadersFleetAttack;
        private readonly Bounds _movementBounds;

        public InvadersFleet(
            InvadersConfig invadersConfig,
            Bounds movementBounds,
            BulletFactory bulletFactory)
        {
            _movementBounds = movementBounds;
            _invadersFleetMovement = new InvadersFleetMovement(invadersConfig, _state, movementBounds);
            _invadersFleetAttack = new InvadersFleetAttack(invadersConfig, _state, bulletFactory);
        }
        
        public IObservable<Invader> InvaderDestroyedAsObservable() => _invaderDestroyedCommand;

        public IObservable<Unit> AllInvadersDestroyedAsObservable() => _allInvadersDestroyedCommand;

        public IObservable<Unit> ReachedBottomAsObservable() => _reachedBottomCommand;

        public void SetAttackSpeedMultiplier(float multiplier)
        {
            _invadersFleetAttack.SetAttackSpeedMultiplier(multiplier);
        }

        public void AddInvader(Invader invader, int rowIndex)
        {
            var safeCounter = 0;
            while (rowIndex >= _state.Rows.Count || safeCounter++ < 100)
                _state.Rows.Add(new List<Invader>());

            _state.Rows[rowIndex].Add(invader);
        }

        public void Init()
        {
            _state.InitialCount = _state.Rows.Sum(x => x.Count);
            _state.CurrentCount = _state.InitialCount;

            _invaderSubscriptions.Clear();
            for (var rowIndex = 0; rowIndex < _state.Rows.Count; rowIndex++)
            {
                var row = _state.Rows[rowIndex];
                var rowIndexCopy = rowIndex;

                foreach (var invader in row)
                {
                    invader
                        .DestroyedAsObservable()
                        .Subscribe(inv => OnInvaderDestroyed(inv, rowIndexCopy))
                        .AddTo(_invaderSubscriptions);
                }
            }
        }

        private void OnInvaderDestroyed(Invader invader, int rowIndex)
        {
            var row = _state.Rows[rowIndex];
            row.Remove(invader);
            _state.CurrentCount--;
            _invaderDestroyedCommand.Execute(invader);
            if (_state.CurrentCount <= 0)
            {
                Assert.IsTrue(_state.Rows.Any(r => r.Count == 0));
                _allInvadersDestroyedCommand.Execute();
            }
        }

        void IDisposable.Dispose()
        {
            _invaderSubscriptions.Dispose();
            _reachedBottomCommand.Dispose();
            _allInvadersDestroyedCommand.Dispose();
            _invaderDestroyedCommand.Dispose();
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateAttack(deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            _invadersFleetMovement.Update(deltaTime);

            if (IsReachedBottom())
            {
                _reachedBottomCommand.Execute();
            }
        }

        private void UpdateAttack(float deltaTime)
        {
            _invadersFleetAttack.Update(deltaTime);
        }

        private bool IsReachedBottom()
        {
            var lastNonEmptyRow = _state.Rows.FindLast(l => l.Count > 0);
            if (lastNonEmptyRow == null)
                return false;

            var invaderFromLastRow = lastNonEmptyRow[0];
            var isReachedBottom = invaderFromLastRow.Position.y < _movementBounds.min.y;
            return isReachedBottom;
        }
    }
}