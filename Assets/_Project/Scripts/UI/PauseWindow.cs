using _Project.Scripts.Services;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class PauseWindow : UiWindow
    {
        [Inject] private PauseService _pauseService;
        [Inject] private SceneLoader _sceneLoader;

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;

        private readonly CompositeDisposable _subscriptions = new();

        protected override void OnInit()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => Continue()).AddTo(_subscriptions);
            _restartButton.OnClickAsObservable().Subscribe(_ => Restart()).AddTo(_subscriptions);
        }

        protected override void OnDisposed()
        {
            _subscriptions.Dispose();
            _pauseService.RemovePauseDemander(this);
        }

        protected override void OnShown()
        {
            _pauseService.AddPauseDemander(this);
        }

        protected override void OnHidden()
        {
            _pauseService.RemovePauseDemander(this);
        }

        private void Continue()
        {
            Debug.Log("[PauseWindow] Continue");
            Hide();
        }

        private void Restart()
        {
            Debug.Log("[PauseWindow] Restart");
            _sceneLoader.ReloadScene();
        }
    }
}