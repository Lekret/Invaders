using _Project.Scripts.Game.Projectiles.Lasers;

namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class LaserShipWeapon : IShipWeapon
    {
        private readonly LaserFactory _laserFactory;
        private readonly ShipProvider _shipProvider;
        private Laser _laser;
        private float _charge;

        public LaserShipWeapon(
            LaserFactory laserFactory, 
            ShipProvider shipProvider)
        {
            _laserFactory = laserFactory;
            _shipProvider = shipProvider;
        }

        public float Charge
        {
            get => _charge;
            set => _charge = value;
        }

        public bool IsEmpty()
        {
            return _charge <= 0f;
        }

        void IShipWeapon.OnEquipped()
        {
            _laser = _laserFactory.CreateLaser();
        }

        void IShipWeapon.OnUnequipped()
        {
            if (_laser != null)
                _laser.Destroy();
        }

        void IShipWeapon.Update(float deltaTime, bool isAttackRequested)
        {
            if (isAttackRequested)
            {
                _laser.IsActive = true;
                _laser.Position = _shipProvider.Ship.Position;
                _charge -= deltaTime;
            }
            else
            {
                _laser.IsActive = false;
            }
        }
    }
}