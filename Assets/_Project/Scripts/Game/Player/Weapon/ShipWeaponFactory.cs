using _Project.Scripts.Game.Player.Weapon.Impl;
using _Project.Scripts.Game.Projectiles;

namespace _Project.Scripts.Game.Player.Weapon
{
    public class ShipWeaponFactory
    {
        private readonly ShipProvider _shipProvider;
        private readonly BulletFactory _bulletFactory;

        public ShipWeaponFactory(
            ShipProvider shipProvider,
            BulletFactory bulletFactory)
        {
            _shipProvider = shipProvider;
            _bulletFactory = bulletFactory;
        }

        public BulletShipWeapon CreateBulletWeapon()
        {
            return new BulletShipWeapon(_bulletFactory, _shipProvider);
        }

        public ShotgunShipWeapon CreateShotgun()
        {
            return null;
        }

        public LaserShipWeapon CreateLaser()
        {
            return null;
        }
    }
}