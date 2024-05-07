using _Project.Scripts.Game.Services;
using _Project.Scripts.UI.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.Pause
{
    public class PauseWindow : UiWindow
    {
        [Inject] private PauseService _pauseService;
        [Inject] private GameRestarter _gameRestarter;

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        
        protected override void OnInit()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => Continue()).AddTo(gameObject);
            _restartButton.OnClickAsObservable().Subscribe(_ => Restart()).AddTo(gameObject);
        }

        protected override void OnDisposed()
        {
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
            Hide();
        }

        private void Restart()
        {
            _gameRestarter.RestartGame();
        }
    }
}