using UnityEngine;

namespace _Project.Scripts.Game.Pickups.Behaviour
{
    public abstract class PickupBehaviour : ScriptableObject, IPickupBehaviour
    {
        [SerializeField] private Sprite _iconSprite;

        public Sprite IconSprite => _iconSprite;
        
        public abstract void Execute();
    }
}