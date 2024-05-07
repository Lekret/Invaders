using UnityEngine;

namespace _Project.Scripts.Game.Pickups
{
    [CreateAssetMenu(menuName = "Config/PickupsConfig")]
    public class PickupsConfig : ScriptableObject
    {
        [SerializeField] private PickupView _pickupViewPrefab;
        [SerializeField] private float _speed;
        [SerializeField, Range(0f, 1f)] private float _pickupSpawnProbability = 0.1f;
        
        public PickupView PickupViewPrefab => _pickupViewPrefab;
        public float Speed => _speed;
        public float PickupSpawnProbability => _pickupSpawnProbability;
    }
}