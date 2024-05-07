using _Project.Scripts.Game.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class ShotgunShipWeapon : IShipWeapon
    {
        private static readonly Vector2[] BulletDirections =
        {
            Vector2.up,
            Quaternion.Euler(0f, 0f, -45f) * Vector2.up,
            Quaternion.Euler(0f, 0f, 45) * Vector3.up
        };

        private readonly BulletFactory _bulletFactory;
        private readonly Ship _ship;
        private float _currentAttackCooldown;

        public ShotgunShipWeapon(
            BulletFactory bulletFactory,
            Ship ship)
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
                foreach (var bulletDirection in BulletDirections)
                {
                    _bulletFactory.CreateBullet(_ship.Position, bulletDirection, Team.Player);
                }
            }
        }
    }
}