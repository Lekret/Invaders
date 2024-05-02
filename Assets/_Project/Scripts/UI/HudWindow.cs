using _Project.Scripts.UI.Core;
using _Project.Scripts.UI.Utils;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class HudWindow : UiWindow
    {
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
                .Subscribe(OnInput)
                .AddTo(_subscriptions);
        }

        private void OnInput(int input)
        {
            Debug.Log($"Input: {input}");
        }

        protected override void OnDisposed()
        {
            _subscriptions.Dispose();
        }
    }
}