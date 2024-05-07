﻿using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    [CreateAssetMenu(menuName = "Config/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int _shipHealth = 5;
        [SerializeField] private float _shipSpeed = 3f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _bulletSpeed = 5f;
        [SerializeField] private ShipView _shipViewPrefab;
        [SerializeField] private BulletView _bulletViewPrefab;

        public int ShipHealth => _shipHealth;
        public float ShipSpeed => _shipSpeed;
        public float AttackCooldown => _attackCooldown;
        public float BulletSpeed => _bulletSpeed;
        public ShipView ShipViewPrefab => _shipViewPrefab;
        public BulletView BulletViewPrefab => _bulletViewPrefab;
    }
}