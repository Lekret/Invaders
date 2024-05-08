using _Project.Scripts.Game.Player.Weapon.Impl;
using _Project.Scripts.Game.Projectiles;

namespace _Project.Scripts.Game.Player.Weapon
{
    public class ShipWeaponFactory
    {
        private readonly ShipProvider _shipProvider;
        private readonly BulletFactory _bulletFactory;
        private readonly PlayerConfig _playerConfig;

        public ShipWeaponFactory(
            ShipProvider shipProvider,
            BulletFactory bulletFactory, 
            PlayerConfig playerConfig)
        {
            _shipProvider = shipProvider;
            _bulletFactory = bulletFactory;
            _playerConfig = playerConfig;
        }

        public RifleShipWeapon CreateRifle()
        {
            var weapon = new RifleShipWeapon(_bulletFactory, _shipProvider);
            weapon.AttackInterval = _playerConfig.RifleAttackInterval;
            weapon.BulletSpeed = _playerConfig.RifleBulletSpeed;
            return weapon;
        }

        public ShotgunShipWeapon CreateShotgunWeapon(
            int ammo, 
            float attackInterval, 
            float bulletSpeed)
        {
            var weapon = new ShotgunShipWeapon(_bulletFactory, _shipProvider);
            weapon.Ammo = ammo;
            weapon.AttackInterval = attackInterval;
            weapon.BulletSpeed = bulletSpeed;
            return weapon;
        }

        public LaserShipWeapon CreateLaser(float charges)
        {
            var weapon = new LaserShipWeapon();
            weapon.Charges = charges;
            return weapon;
        }
    }
}