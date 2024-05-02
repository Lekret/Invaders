using UnityEngine;

namespace _Project.Scripts.UI.Core
{
    [CreateAssetMenu(menuName = "Config/WindowsConfig")]
    public class WindowsConfig : ScriptableObject
    {
        [SerializeField] private GameObject[] _windowPrefabs;

        public GameObject[] WindowPrefabs => _windowPrefabs;
    }
}