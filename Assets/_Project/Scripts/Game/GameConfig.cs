using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;

namespace _Project.Scripts.Game
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private BulletView _bulletViewPrefab;
        
        public BulletView BulletViewPrefab => _bulletViewPrefab;
    }
}