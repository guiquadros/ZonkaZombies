namespace ZonkaZombies.Characters.Player.Weapon
{
    public interface IWeaponSelector
    {
        bool Select(WeaponModel weapon);
        bool Discard();
        void TryToUse();
        bool IsEquippingFireGun { get; }
        WeaponDetails GetWeapon();
    }
}