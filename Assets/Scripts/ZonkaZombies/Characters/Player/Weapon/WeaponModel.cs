using System;
using UnityEngine;

namespace ZonkaZombies.Characters.Player.Weapon
{
    public class WeaponModel
    {
        public WeaponDetails Details;
        public GameObject GameObject;
        public Action OnReset;

        public WeaponModel(WeaponDetails details, GameObject gameObject)
        {
            Details = details;
            GameObject = gameObject;
        }

        public void Reset()
        {
            if (OnReset != null)
            {
                OnReset();
            }
        }
    }
}
