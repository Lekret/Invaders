using _Project.Scripts.Game.Services;
using _Project.Scripts.UI.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class GameOutcomeWindow : UiWindow
    {
        [Inject] private PauseService _pauseService;
        [Inject] private GameRestarter _gameRestarter;

        [SerializeField] private TextMeshProUGUI _outcomeStatusText;
        [SerializeField] private Color _winColor = Color.green;
        [SerializeField] private Color _loseColor = Color.red;
        [SerializeField] private Button _restartButton;

        public void ConfigureAsWin()
        {
            _outcomeStatusText.text = "Win!";
            _outcomeStatusText.color = _winColor;
        }

        public void ConfigureAsLose()
        {
            _outcomeStatusText.text = "Game Over";
            _outcomeStatusText.color = _loseColor;
        }

        protected override void OnInit()
        {
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

        private void Restart()
        {
            _gameRestarter.RestartGame();
        }
    }
}