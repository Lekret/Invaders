using _Project.Scripts.Game.Events;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.Hud
{
    public class ScoreView : MonoBehaviour
    {
        [Inject] private IMessageReceiver _messageReceiver;

        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init()
        {
            SetScore(0);
            _messageReceiver
                .Receive<PlayerScoreChangedEvent>()
                .Subscribe(e => SetScore(e.NewScore))
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