using UnityEngine;

namespace ZonkaZombies.Characters.Player.Weapon
{
    public abstract class BaseWeaponSelector : MonoBehaviour, IWeaponSelector
    {
        public abstract bool Select(WeaponModel weapon);
        public abstract bool Discard();
        public abstract bool IsEquippingFireGun { get; }
        public abstract WeaponDetails GetWeapon();
        public abstract void TryToUse();
    }
}