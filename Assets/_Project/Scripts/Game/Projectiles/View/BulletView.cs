using UniRx;
using UnityEngine;

namespace _Project.Scripts.Game.Projectiles.View
{
    public class BulletView : MonoBehaviour
    {
        private readonly CompositeDisposable _subscriptions = new();
        
        public void Init(Bullet bullet)
        {
            bullet
                .PositionAsObservable()
                .Subscribe(x => transform.position = x)
                .AddTo(_subscriptions);
            
            _subscriptions.AddTo(this);
        }
    }
}