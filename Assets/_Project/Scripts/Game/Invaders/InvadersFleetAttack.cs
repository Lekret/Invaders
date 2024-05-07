using _Project.Scripts.Game.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetAttack
    {
        private readonly InvadersConfig _invadersConfig;
        private readonly InvadersFleetState _state;
        private readonly BulletFactory _bulletFactory;
        private float _timeUntilAttack;
        
        public InvadersFleetAttack(
            InvadersConfig invadersConfig, 
            InvadersFleetState state, 
            BulletFactory bulletFactory)
        {
            _invadersConfig = invadersConfig;
            _state = state;
            _bulletFactory = bulletFactory;
        }

        public void Update(float deltaTime)
        {
            if (_timeUntilAttack > 0f)
            {
                _timeUntilAttack -= deltaTime;
                if (_timeUntilAttack > 0f)
                    return;
            }

            _timeUntilAttack = _invadersConfig.AttackInterval;

            var invaderPosition = GetShooterInvaderPosition();
            if (invaderPosition.HasValue)
            {
                var bullet = _bulletFactory.CreateBullet(BulletType.InvaderBullet);
                bullet.Team = Team.Invaders;
                bullet.Position = invaderPosition.Value;
                bullet.Velocity = Vector2.down * 5f;
            }
        }

        private Vector3? GetShooterInvaderPosition()
        {
            foreach (var row in _state.Rows)
            {
                if (row.Count == 0)
                    continue;

                var rndIndex = Random.Range(0, row.Count);
                var rndIvader = row[rndIndex];
                return rndIvader.Position;
            }

            return null;
        }
    }
}