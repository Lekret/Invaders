using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    [CreateAssetMenu(menuName = "Config/InvadersConfig")]
    public class InvadersConfig : ScriptableObject
    {
        [SerializeField] private int _rowCount = 11;
        [SerializeField] private int _columnCount = 5;
        [SerializeField] private float _spawnHorizontalSpacing = 0.3f;
        [SerializeField] private float _spawnVerticalSpacing = 0.3f;
        [SerializeField] private InvaderView _invaderViewPrefab;
        [SerializeField] private BulletView _bulletViewPrefab;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;
        public float SpawnHorizontalSpacing => _spawnVerticalSpacing;
        public float SpawnVerticalSpacing => _spawnVerticalSpacing;
        public InvaderView InvaderViewPrefab => _invaderViewPrefab;
        public BulletView BulletViewPrefab => _bulletViewPrefab;
    }
}