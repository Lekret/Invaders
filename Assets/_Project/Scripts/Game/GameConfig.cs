using _Project.Scripts.Game.Projectiles;
using _Project.Scripts.Game.Projectiles.View;
using UnityEngine;

namespace _Project.Scripts.Game
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float _availableScreenArea = 0.8f;

        public float AvailableScreenArea => _availableScreenArea;
    }
}