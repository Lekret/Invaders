using System;
using _Project.Scripts.Game.Core;
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
        private bool _uiWantsAttack;
        private float _uiInputDelta;

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
                .Subscribe(_ => _uiWantsAttack = true)
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
            var movementDelta = ReadMovementDeltaInput();
            var wantsAttack = ReadWantsAttackInput();

            if (_inputListener != null)
            {
                _inputListener.SetMovementDeltaInput(movementDelta);
                
                if (wantsAttack)
                    _inputListener.OnAttackInput();
            }
        }

        private float ReadMovementDeltaInput()
        {
            var kbInput = Input.GetAxisRaw("Horizontal");
            var movementDelta = kbInput + _uiInputDelta;
            movementDelta = Mathf.Clamp(movementDelta, -1f, 1f);
            return movementDelta;
        }

        private bool ReadWantsAttackInput()
        {
            var wantsAttack = _uiWantsAttack || Input.GetKeyDown(KeyCode.Space);
            _uiWantsAttack = false;
            return wantsAttack;
        }
    }
}