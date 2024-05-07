using _Project.Scripts.Game.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class BulletShipWeapon : IShipWeapon
    {
        private readonly BulletFactory _bulletFactory;
        private readonly ShipProvider _ship;
        private float _currentAttackCooldown;

        public BulletShipWeapon(
            BulletFactory bulletFactory, 
            ShipProvider ship)
        {
            _bulletFactory = bulletFactory;
            _ship = ship;
        }

        public void Update(float deltaTime, bool isAttackRequested)
        {
            if (_currentAttackCooldown > 0f)
            {
                _currentAttackCooldown -= deltaTime;
                if (_currentAttackCooldown > 0f)
                    return;
            }
            
            if (isAttackRequested)
            {
                _currentAttackCooldown = 0.5f;
                _bulletFactory.CreateBullet(_ship.Ship.Position, Vector2.up, Team.Player);
            }
        }
    }
}