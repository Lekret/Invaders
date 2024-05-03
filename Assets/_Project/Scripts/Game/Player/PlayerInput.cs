using System;
using _Project.Scripts.Events;
using _Project.Scripts.Game.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.Game.Player
{
    public class PlayerInput : IUpdatable, IAwakeable, IDisposable
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

        void IAwakeable.OnAwake()
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
            var kbInput = Input.GetAxisRaw("Horizontal");
            var totalMovementDelta = kbInput + _uiInputDelta;
            totalMovementDelta = Mathf.Clamp(totalMovementDelta, -1f, 1f);
            _uiInputDelta = 0f;

            var wantsAttack = _uiWantsAttack || Input.GetKeyDown(KeyCode.Space);
            _uiWantsAttack = false;

            if (_inputListener != null)
            {
                _inputListener.SetMovementDelta(totalMovementDelta);
                
                if (wantsAttack)
                    _inputListener.SetWantsAttack();
            }
        }
    }
}