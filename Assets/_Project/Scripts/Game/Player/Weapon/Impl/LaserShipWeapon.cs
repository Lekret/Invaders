namespace _Project.Scripts.Game.Player.Weapon.Impl
{
    public class LaserShipWeapon : IShipWeapon
    {
        private float _charges;

        public float Charges
        {
            get => _charges;
            set => _charges = value;
        }

        public bool IsEmpty()
        {
            return _charges <= 0f;
        }

        void IShipWeapon.OnEquipped()
        {
            _charges = 120f;
        }

        void IShipWeapon.OnUnequipped()
        {
        }

        void IShipWeapon.Update(float deltaTime, bool isAttackRequested)
        {
            _charges -= deltaTime;
        }
    }
}