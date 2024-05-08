using _Project.Scripts.Game.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class RifleShipWeapon : IShipWeapon
    {
        private readonly BulletFactory _bulletFactory;
        private readonly ShipProvider _ship;
        private float _attackInterval;
        private float _attackCooldown;
        private float _bulletSpeed;

        public RifleShipWeapon(
            BulletFactory bulletFactory, 
            ShipProvider ship)
        {
            _bulletFactory = bulletFactory;
            _ship = ship;
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
            return false;
        }

        void IShipWeapon.OnEquipped()
        {
        }

        void IShipWeapon.OnUnequipped()
        {
        }

        void IShipWeapon.Update(float deltaTime, bool isAttackRequested)
        {
            if (_attackCooldown > 0f)
            {
                _attackCooldown -= deltaTime;
                if (_attackCooldown > 0f)
                    return;
            }
            
            if (isAttackRequested)
            {
                _attackCooldown = _attackInterval;
                _bulletFactory.CreateBullet(
                    _ship.Ship.Position, 
                    Vector2.up * _bulletSpeed, 
                    Team.Player);
            }
        }
    }
}