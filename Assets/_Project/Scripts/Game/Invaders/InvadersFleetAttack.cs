﻿using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Projectiles.Bullets;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    public class InvadersFleetAttack
    {
        private readonly InvadersConfig _invadersConfig;
        private readonly InvadersFleetState _state;
        private readonly BulletFactory _bulletFactory;
        private float _timeUntilAttack;
        private float _attackSpeedMultiplier = 1f;
        
        public InvadersFleetAttack(
            InvadersConfig invadersConfig, 
            InvadersFleetState state, 
            BulletFactory bulletFactory)
        {
            _invadersConfig = invadersConfig;
            _state = state;
            _bulletFactory = bulletFactory;
        }
        
        public void SetAttackSpeedMultiplier(float multiplier)
        {
            _attackSpeedMultiplier = multiplier;
        }

        public void Update(float deltaTime)
        {
            if (_timeUntilAttack > 0f)
            {
                _timeUntilAttack -= deltaTime;
                if (_timeUntilAttack > 0f)
                    return;
            }

            _timeUntilAttack = _invadersConfig.AttackInterval / _attackSpeedMultiplier;

            var invaderPosition = GetShooterInvaderPosition();
            if (invaderPosition.HasValue)
            {
                _bulletFactory.CreateBullet(
                    invaderPosition.Value, 
                    Vector2.down * _invadersConfig.BulletSpeed, 
                    Team.Invaders);
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