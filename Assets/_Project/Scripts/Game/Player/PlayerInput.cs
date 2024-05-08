using System;
using _Project.Scripts.Game.CoreLoop;
using _Project.Scripts.Game.Events;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    public class PlayerInput : IUpdatable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly IMessageReceiver _messageReceiver;
        private IInputListener _inputListener;
        private bool _isUiAttackPressed;
        private Vector2 _uiInputDelta;

        public PlayerInput(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
        }

        public void SetInputListener(IInputListener inputListener)
        {
            _inputListener = inputListener;
        }

        public void Init()
        {
            _messageReceiver
                .Receive<UiAttackInputEvent>()
                .Subscribe(e => _isUiAttackPressed = e.IsPressed)
                .AddTo(_subscriptions);

            _messageReceiver
                .Receive<UiMovementInputEvent>()
                .Subscribe(e => _uiInputDelta = e.Delta)
                .AddTo(_subscriptions);
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            if (_inputListener == null) 
                return;
            
            var movementDelta = GetMovementDelta();
            _inputListener.SetMovementDelta(movementDelta);
                
            var isAttackRequested = IsAttackRequested();
            if (isAttackRequested)
                _inputListener.OnAttackRequested();
        }

        private Vector2 GetMovementDelta()
        {
            var kbInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var movementDelta = kbInput + _uiInputDelta;
            movementDelta = Vector2.ClampMagnitude(movementDelta, 1f);
            return movementDelta;
        }

        private bool IsAttackRequested()
        {
            var wantsAttack = _isUiAttackPressed || Input.GetKey(KeyCode.Space);
            return wantsAttack;
        }
    }
}