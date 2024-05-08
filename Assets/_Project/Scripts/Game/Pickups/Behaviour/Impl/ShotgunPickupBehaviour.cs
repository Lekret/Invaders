using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.Weapon;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Pickups.Behaviour.Impl
{
    [CreateAssetMenu(menuName = "Config/Pickups/ShotgunPickup")]
    public class ShotgunPickupBehaviour : PickupBehaviour
    {
        [Inject] private ShipProvider _shipProvider;
        [Inject] private ShipWeaponFactory _shipWeaponFactory;

        [SerializeField] private int _shotgunAmmo = 6;
        [SerializeField] private float _attackInterval = 0.5f;
        [SerializeField] private float _bulletSpeed = 5f;
        
        public override void Execute()
        {
            _shipProvider.Ship.Weapon = _shipWeaponFactory.CreateShotgunWeapon(
                _shotgunAmmo, 
                _attackInterval, 
                _bulletSpeed);
        }
    }
}