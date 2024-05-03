using _Project.Scripts.Events;
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
        [Inject] private IMessageBroker _messageBroker;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private TouchButton _attackButton;
        [SerializeField] private TouchButton _moveLeftButton;
        [SerializeField] private TouchButton _moveRightButton;

        private readonly CompositeDisposable _subscriptions = new();

        protected override void OnInit()
        {
            _attackButton
                .OnPointerDownAsObservable()
                .Subscribe(_ => SendAttackEvent())
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

        private void SendAttackEvent()
        {
            _messageBroker.Publish(new UiAttackInputEvent());
        }

        private void SendMovementInput(float delta)
        {
            _messageBroker.Publish(new UiMovementInputEvent(delta));
        }

        private void PauseGame()
        {
            _messageBroker.ShowWindow<PauseWindow>();
        }

        protected override void OnDisposed()
        {
            _subscriptions.Dispose();
        }
    }
}