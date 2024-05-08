using _Project.Scripts.Game.CoreLoop;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Pickups
{
    public class PickupFactory
    {
        private readonly GameLoop _gameLoop;
        private readonly IInstantiator _instantiator;
        private readonly PickupsConfig _pickupsConfig;

        public PickupFactory(
            GameLoop gameLoop, 
            IInstantiator instantiator,
            PickupsConfig pickupsConfig)
        {
            _gameLoop = gameLoop;
            _instantiator = instantiator;
            _pickupsConfig = pickupsConfig;
        }

        public Pickup CreateRandomPickup(Vector3 position)
        {
            var pickup = new Pickup();
            pickup.Position = position;
            pickup.Velocity = Vector2.down * _pickupsConfig.Speed;
            var pickupBehaviour = _pickupsConfig.Behaviours[Random.Range(0, _pickupsConfig.Behaviours.Length)];
            pickup.SetPickupBehaviour(pickupBehaviour);
            
            var pickupView = _instantiator.InstantiatePrefabForComponent<PickupView>(_pickupsConfig.PickupViewPrefab);
            pickupView.Init(pickup, pickupBehaviour.IconSprite);
            pickup
                .DestroyedAsObservable()
                .Subscribe(_ => _gameLoop.Remove(pickup))
                .AddTo(pickup.Subscriptions);

            _gameLoop.Add(pickup);
            pickup.Init();
            return pickup;
        }
    }
}