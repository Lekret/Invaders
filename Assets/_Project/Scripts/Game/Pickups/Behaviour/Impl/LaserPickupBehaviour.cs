using _Project.Scripts.Game.Player;
using _Project.Scripts.Game.Player.Weapon;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Pickups.Behaviour.Impl
{
    [CreateAssetMenu(menuName = "Config/Pickups/LaserPickup")]
    public class LaserPickupBehaviour : PickupBehaviour
    {
        [Inject] private ShipProvider _shipProvider;
        [Inject] private ShipWeaponFactory _shipWeaponFactory;

        [SerializeField] private float _laserCharges = 60f;
        
        public override void Execute()
        {
            _shipProvider.Ship.Weapon = _shipWeaponFactory.CreateLaser(_laserCharges);
        }
    }
}