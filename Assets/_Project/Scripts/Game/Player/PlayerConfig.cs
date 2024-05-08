using _Project.Scripts.Game.Player.View;
using _Project.Scripts.Game.Projectiles.Bullets.View;
using _Project.Scripts.Game.Projectiles.Lasers.View;
using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    [CreateAssetMenu(menuName = "Config/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int _shipHealth = 5;
        [SerializeField] private float _shipSpeed = 3f;
        [SerializeField] private float _rifleBulletSpeed = 5f;
        [SerializeField] private float _rifleAttackInterval = 0.5f;
        [SerializeField] private ShipView _shipViewPrefab;
        [SerializeField] private BulletView _bulletViewPrefab;
        [SerializeField] private LaserView _laserViewPrefab;

        public int ShipHealth => _shipHealth;
        public float ShipSpeed => _shipSpeed;
        public float RifleBulletSpeed => _rifleBulletSpeed;
        public float RifleAttackInterval => _rifleAttackInterval;
        public ShipView ShipViewPrefab => _shipViewPrefab;
        public BulletView BulletViewPrefab => _bulletViewPrefab;
        public LaserView LaserViewPrefab => _laserViewPrefab;
    }
}