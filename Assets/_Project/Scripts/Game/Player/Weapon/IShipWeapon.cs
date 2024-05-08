using System;

namespace _Project.Scripts.Game.Player.Weapon
{
    public interface IShipWeapon
    {
        bool IsEmpty();
        void OnEquipped();
        void OnUnequipped();
        void Update(float deltaTime, bool isAttackRequested);
    }
}