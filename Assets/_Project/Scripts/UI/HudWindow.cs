using _Project.Scripts.Game.Events;
using _Project.Scripts.UI.Core;
using _Project.Scripts.UI.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class HudWindow : UiWindow
    {
        [Inject] private IMessagePublisher _messagePublisher;

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TouchButton _attackButton;
        [SerializeField] private TouchButton _moveLeftButton;
        [SerializeField] private TouchButton _moveRightButton;

        private readonly CompositeDisposable _subscriptions = new();

        protected override void OnInit()
        {
            _healthBar.Init();
            
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
                    _moveLeftButton.OnPointerDownAsObservable().Select(_ => -1),
                    _moveLeftButton.OnPointerUpAsObservable().Select(_ => 1),
                    _moveRightButton.OnPointerDownAsObservable().Select(_ => 1),
                    _moveRightButton.OnPointerUpAsObservable().Select(_ => -1))
                .Scan(
                    seed: 0,
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

        private void SendMovementInput(float delta)
        {
            _messagePublisher.Publish(new UiMovementInputEvent(delta));
        }

        private void PauseGame()
        {
            _messagePublisher.ShowWindow<PauseWindow>();
        }

        protected override void OnDisposed()
        {
            _healthBar.Dispose();
            _subscriptions.Dispose();
        }
    }
}