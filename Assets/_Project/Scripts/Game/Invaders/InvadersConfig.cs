using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Projectiles.Bullets.View;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    [CreateAssetMenu(menuName = "Config/InvadersConfig")]
    public class InvadersConfig : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField] private int _countInRow = 11;
        [SerializeField] private int _countInColumn = 5;
        [SerializeField] private float _spawnHorizontalSpacing = 0.3f;
        [SerializeField] private float _spawnVerticalSpacing = 0.3f;
        [Header("Movement")]
        [SerializeField] private float _minMovementInterval = 0.1f;
        [SerializeField] private float _maxMovementInterval = 2f;
        [SerializeField] private float _horizontalMovementPerTick = 0.1f;
        [SerializeField] private float _verticalMovementPerReachedSide = 0.5f;
        [SerializeField] private float _reachedPlayerToleranceY = 0.5f;
        [Header("Attack")] 
        [SerializeField] private float _attackInterval = 3f;
        [SerializeField] private float _bulletSpeed = 5f;
        [Header("Prefabs")]
        [SerializeField] private InvaderView _invaderViewPrefab;
        [SerializeField] private BulletView _bulletViewPrefab;
        [Header("Skin")] 
        [SerializeField] private InvaderSkin[] _skinsPerRow;
        
        public int CountInRow => _countInRow;
        public int CountInColumn => _countInColumn;
        public float SpawnHorizontalSpacing => _spawnHorizontalSpacing;
        public float SpawnVerticalSpacing => _spawnVerticalSpacing;

        public float MinMovementInterval => _minMovementInterval;
        public float MaxMovementInterval => _maxMovementInterval;
        public float HorizontalMovementPerTick => _horizontalMovementPerTick;
        public float VerticalMovementPerReachedSide => _verticalMovementPerReachedSide;
        public float ReachedPlayerToleranceY => _reachedPlayerToleranceY;

        public float AttackInterval => _attackInterval;
        public float BulletSpeed => _bulletSpeed;
        
        public InvaderView InvaderViewPrefab => _invaderViewPrefab;
        public BulletView BulletViewPrefab => _bulletViewPrefab;

        public InvaderSkin GetSkinByRow(int rowIndex)
        {
            var cycledIndex = rowIndex % _skinsPerRow.Length;
            var skin = _skinsPerRow[cycledIndex];
            return skin;
        }
    }
}