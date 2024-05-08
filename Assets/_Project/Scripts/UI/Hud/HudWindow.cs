using System;
using System.Numerics;
using _Project.Scripts.Game.Events;
using _Project.Scripts.UI.Core;
using _Project.Scripts.UI.Pause;
using _Project.Scripts.UI.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Vector2 = UnityEngine.Vector2;

namespace _Project.Scripts.UI.Hud
{
    public class HudWindow : UiWindow
    {
        [Inject] private IMessagePublisher _messagePublisher;

        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TouchButton _attackButton;
        [SerializeField] private TouchButton _moveLeftButton;
        [SerializeField] private TouchButton _moveRightButton;
        [SerializeField] private TouchButton _moveDownButton;
        [SerializeField] private TouchButton _moveUpButton;

        private readonly CompositeDisposable _subscriptions = new();

        protected override void OnInit()
        {
            _healthBarView.Init();
            _scoreView.Init();

            _attackButton
                .OnPointerDownAsObservable()
                .Subscribe(_ => SendAttackEvent(true))
                .AddTo(_subscriptions);

            _attackButton
                .OnPointerUpAsObservable()
                .Subscribe(_ => SendAttackEvent(false))
                .AddTo(_subscriptions);

            Observable
                .Merge(
                    new[]
                    {
                        _moveLeftButton.OnPointerDownAsObservable().Select(_ => Vector2.left),
                        _moveLeftButton.OnPointerUpAsObservable().Select(_ => Vector2.right),

                        _moveRightButton.OnPointerDownAsObservable().Select(_ => Vector2.right),
                        _moveRightButton.OnPointerUpAsObservable().Select(_ => Vector2.left),

                        _moveDownButton.OnPointerDownAsObservable().Select(_ => Vector2.down),
                        _moveDownButton.OnPointerUpAsObservable().Select(_ => Vector2.up),

                        _moveUpButton.OnPointerDownAsObservable().Select(_ => Vector2.up),
                        _moveUpButton.OnPointerUpAsObservable().Select(_ => Vector2.down)
                    })
                .Scan(
                    seed: Vector2.zero,
                    accumulator: (acc, x) => acc + x)
                .Subscribe(x => SendMovementInput(x))
                .AddTo(_subscriptions);

            _pauseButton
                .OnClickAsObservable()
                .Subscribe(_ => PauseGame())
                .AddTo(_subscriptions);
        }

        private void SendAttackEvent(bool isPressed)
        {
            _messagePublisher.Publish(new UiAttackInputEvent(isPressed));
        }

        private void SendMovementInput(Vector2 delta)
        {
            _messagePublisher.Publish(new UiMovementInputEvent(delta));
        }

        private void PauseGame()
        {
            _messagePublisher.ShowWindow<PauseWindow>();
        }

        protected override void OnDisposed()
        {
            _healthBarView.Dispose();
            _scoreView.Dispose();
            _subscriptions.Dispose();
        }
    }
}