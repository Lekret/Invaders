using UnityEngine;

namespace _Project.Scripts.Game.Player
{
    [CreateAssetMenu(menuName = "Config/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _shipSpeed = 3f;
        [SerializeField] private float _attackCooldown = 2f;
        [SerializeField] private GameObject _shipViewPrefab;

        public float ShipSpeed => _shipSpeed;
        public float AttackCooldown => _attackCooldown;
        public GameObject ShipViewPrefab => _shipViewPrefab;
    }
}