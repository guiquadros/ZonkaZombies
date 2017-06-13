using UnityEngine;
using ZonkaZombies.Characters.Player.Weapon;

namespace ZonkaZombies.Scenery.Interaction
{
    public class WeaponInteractable : CollectableInteractable
    {
        [SerializeField]
        private WeaponDetails _weaponDetails;

        private WeaponModel _model;

        public override void Collect()
        {
            gameObject.SetActive(false);
            if (_model == null)
            {
                _model = new WeaponModel(_weaponDetails, gameObject) {OnReset = OnReset};
            }
            DispatchOnInteractEvent(PlayerInteracting, _model);
        }

        private void OnReset()
        {
            ResetInteractable();
        }
    }
}