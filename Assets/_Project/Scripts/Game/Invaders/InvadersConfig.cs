using _Project.Scripts.Game.Invaders.View;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;

namespace _Project.Scripts.Game.Invaders
{
    [CreateAssetMenu(menuName = "Config/InvadersConfig")]
    public class InvadersConfig : ScriptableObject
    {
        [SerializeField] private InvaderView _invaderViewPrefab;
        [SerializeField] private BulletView _bulletViewPrefab;

        public InvaderView InvaderViewPrefab => _invaderViewPrefab;
        public BulletView BulletViewPrefab => _bulletViewPrefab;
    }
}