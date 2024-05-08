using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Projectiles.Bullets;
using UnityEngine;

namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class ShotgunShipWeapon : IShipWeapon
    {
        private static readonly Vector2[] BulletDirections =
        {
            Vector2.up,
            Quaternion.Euler(0f, 0f, -25f) * Vector3.up,
            Quaternion.Euler(0f, 0f, 25) * Vector3.up
        };

        private readonly BulletFactory _bulletFactory;
        private readonly ShipProvider _shipProvider;
        private float _attackCooldown;
        private float _attackInterval;
        private float _bulletSpeed;
        private int _ammoLeft;

        public ShotgunShipWeapon(
            BulletFactory bulletFactory,
            ShipProvider shipProvider)
        {
            _bulletFactory = bulletFactory;
            _shipProvider = shipProvider;
        }

        public int Ammo
        {
            get => _ammoLeft;
            set => _ammoLeft = value;
        }

        public float AttackInterval
        {
            get => _attackInterval;
            set => _attackInterval = value;
        }
        
        public float BulletSpeed
        {
            get => _bulletSpeed;
            set => _bulletSpeed = value;
        }

        public bool IsEmpty()
        {
            return _ammoLeft <= 0;
        }

        void IShipWeapon.OnEquipped()
        {
        }

        void IShipWeapon.OnUnequipped()
        {
        }

        void IShipWeapon.Update(float deltaTime, bool isAttackRequested)
        {
            if (_ammoLeft <= 0f)
                return;
            
            if (_attackCooldown > 0f)
            {
                _attackCooldown -= deltaTime;
                if (_attackCooldown > 0f)
                    return;
            }

            if (isAttackRequested)
            {
                _attackCooldown = _attackInterval;
                _ammoLeft--;
                foreach (var bulletDirection in BulletDirections)
                {
                    _bulletFactory.CreateBullet(
                        _shipProvider.Ship.Position, 
                        bulletDirection * _bulletSpeed, 
                        Team.Player);
                }
            }
        }
    }
}