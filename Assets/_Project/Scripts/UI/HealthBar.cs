using _Project.Scripts.Game.Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Inject] private IMessageReceiver _messageReceiver;

        [SerializeField] private Image _healthImage;
        [SerializeField] private Sprite[] _spritePerHealth;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init()
        {
            _messageReceiver
                .Receive<ShipHealthChangedEvent>()
                .Subscribe(OnShipHealthChanged)
                .AddTo(_subscriptions);
        }

        private void OnShipHealthChanged(ShipHealthChangedEvent e)
        {
            if (e.CurrentValue >= _spritePerHealth.Length)
            {
                Debug.LogError($"[HealthBar] Not enough sprites for showing ship health: {e.CurrentValue}");
                return;
            }
            
            _healthImage.sprite = _spritePerHealth[e.CurrentValue];
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}