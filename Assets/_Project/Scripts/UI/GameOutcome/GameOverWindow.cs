using System.Collections;
using _Project.Scripts.Game;
using _Project.Scripts.Game.Services;
using _Project.Scripts.UI.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.GameOutcome
{
    public class GameOverWindow : UiWindow
    {
        [Inject] private PauseService _pauseService;
        [Inject] private GameRestarter _gameRestarter;
        [Inject] private PlayerScoreCounter _playerScoreCounter;

        [SerializeField] private CanvasGroup _contentCanvasGroup;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;

        private Coroutine _animateContentAlpha;

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
            _scoreText.SetText("Score: {0}", _playerScoreCounter.Score);
            
            if (_animateContentAlpha != null)
                StopCoroutine(_animateContentAlpha);
            
            _animateContentAlpha = StartCoroutine(AnimateContentAlpha(fromAlpha: 0f, toAlpha: 1f, duration: 2f));
        }

        private IEnumerator AnimateContentAlpha(float fromAlpha, float toAlpha, float duration)
        {
            _contentCanvasGroup.alpha = fromAlpha;

            var time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                _contentCanvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, time / duration);
                yield return null;
            }
        }

        protected override void OnHidden()
        {
            if (_animateContentAlpha != null)
                StopCoroutine(_animateContentAlpha);
            
            _pauseService.RemovePauseDemander(this);
        }

        private void Restart()
        {
            _gameRestarter.RestartGame();
        }
    }
}