using System;

namespace _Project.Scripts.Game.Player.Weapon
{
    public interface IShipWeapon
    {
        void Update(float deltaTime, bool isAttackRequested);
    }
}