using UnityEngine;

namespace ZonkaZombies.Characters.Player.Weapon
{
    [CreateAssetMenu(fileName = "weaponDetails", menuName = "ZonkaZombies/Weapon/WeaponDetails")]
    public class WeaponDetails : ScriptableObject
    {
        public int WeaponId { get { return (int)WeaponSubType; } }
        [Header("General")]
        public WeaponSubType WeaponSubType;
        public WeaponType Type;
        [Header("White Weapon Config")]
        [Range(1, 200)]
        public int HitPoints;
        [Header("Fire Weapon Config")]
        [Range(1, 20)]
        public int ShotHitPoints;
        [Range(0, 10)]
        public float TimeBetweenBullets = 0.15f; // The time between each shot.
        [Range(0,500)]
        public float Range = 100f; // The distance the gun can fire.
        public AudioClip ShootAudioEffect;
    }
}