using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    [CreateAssetMenu(menuName = "Config/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _attackCooldown;
        [SerializeField] private GameObject _shipViewPrefab;

        public float AttackCooldown => _attackCooldown;
        public GameObject ShipViewPrefab => _shipViewPrefab;
    }
}