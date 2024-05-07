using _Project.Scripts.Game.Services;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.Hud
{
    public class ScoreView : MonoBehaviour
    {
        [Inject] private PlayerScoreCounter _playerScoreCounter;

        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init()
        {
            SetScore(_playerScoreCounter.Score);
            _playerScoreCounter
                .ScoreAsObservable()
                .Subscribe(SetScore)
                .AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        private void SetScore(int score)
        {
            _scoreText.SetText("{0}", score);
        }
    }
}