using _Project.Scripts.Game.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Game
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private BulletView _bulletPrefab;
        
        public BulletView BulletPrefab => _bulletPrefab;
    }
}