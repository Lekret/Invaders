using _Project.Scripts.Game.Core;
using _Project.Scripts.Game.Player.View;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class Ship : IUpdatable, IStartable, IInputListener
    {
        private readonly IShipView _shipView;
        private bool _inputWantsAttack;
        private float _inputMovementDelta;
        private float _attackCooldown;

        public Ship(IShipView shipView)
        {
            _shipView = shipView;
        }

        void IInputListener.SetWantsAttack()
        {
            _inputWantsAttack = true;
        }

        void IInputListener.SetMovementDelta(float movementDelta)
        {
            _inputMovementDelta = Mathf.Clamp(movementDelta, -1f, 1f);
        }

        void IStartable.OnStart()
        {
            // TODO Place to starting position
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            Move(_inputMovementDelta, deltaTime);
            UpdateAttackCooldown(deltaTime);
            
            if (CanAttack())
            {
                ResetAttackCooldown();
                Attack();
            }
            
            ResetWantsAttack();
        }

        private void Move(float movementDelta, float deltaTime)
        {
            var position = _shipView.Position;
            position.x += movementDelta * deltaTime;
            _shipView.Position += position;
        }

        private void UpdateAttackCooldown(float deltaTime)
        {
            if (_attackCooldown > 0f)
                _attackCooldown -= deltaTime;
        }

        private bool CanAttack()
        {
            return _inputWantsAttack && _attackCooldown <= 0f;
        }

        private void ResetWantsAttack()
        {
            _inputWantsAttack = false;
        }

        private void ResetAttackCooldown()
        {
            _attackCooldown = 5f;
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }
    }
}