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
        [SerializeField] private TouchButton _moveLeftButton;
        [SerializeField] private TouchButton _moveRightButton;

        private readonly CompositeDisposable _subscriptions = new();

        protected override void OnInit()
        {
            Observable
                .Merge(
                    _moveLeftButton.OnPointerDownAsObservable().Select(_ => -1),
                    _moveLeftButton.OnPointerUpAsObservable().Select(_ => 1),
                    _moveRightButton.OnPointerDownAsObservable().Select(_ => 1),
                    _moveRightButton.OnPointerUpAsObservable().Select(_ => -1))
                .Scan(
                    seed: 0,
                    accumulator: (acc, x) => acc + x)
                .Subscribe(HandleInput)
                .AddTo(_subscriptions);

            _pauseButton.OnClickAsObservable().Subscribe(_ => PauseGame()).AddTo(_subscriptions);
        }

        private void PauseGame()
        {
            _messageBroker.ShowWindow<PauseWindow>();
        }

        private void HandleInput(int input)
        {
            Debug.Log($"Input: {input}");
        }

        protected override void OnDisposed()
        {
            _subscriptions.Dispose();
        }
    }
}