using UnityEngine;
using ZonkaZombies.Input;
using ZonkaZombies.Managers;

namespace ZonkaZombies.Characters.Player.Weapon
{
    public class WeaponSelector : BaseWeaponSelector
    {
        [SerializeField]
        private PlayerhandIkHandler _handIkHandler;

        // Reference to the player this WeaponSelector belongs
        private Behaviors.Player _player;

        private WeaponModel _weaponModel;
        private WeaponHandler _weaponHandler;

        public override bool IsEquippingFireGun
        {
            get { return _weaponModel != null && _weaponModel.Details.Type == WeaponType.Fire; }
        }

        public void Initialize(Behaviors.Player player, InputReader inputReader)
        {
            _player = player;

            for (int index = 0; index < transform.childCount; index++)
            {
                WeaponHandler weaponHandler = transform.GetChild(index).GetComponent<WeaponHandler>();
                if (weaponHandler != null)
                {
                    weaponHandler.Initialize(inputReader);
                }
            }
        }

        public override bool Select(WeaponModel weapon)
        {
            bool canEquip = _weaponModel == null || _weaponModel.Details.WeaponId != weapon.Details.WeaponId;

            if (canEquip)
            {
                Discard();

                Equip(weapon);
            }

            return canEquip;
        }

        public override bool Discard()
        {
            bool canDiscard = _weaponModel != null && _weaponModel.GameObject != null;

            if (canDiscard)
            {
                SetSelectedGunState(false);
                _weaponModel.GameObject.transform.position = _player.transform.position - Vector3.down * .5f;
                _weaponModel.GameObject.SetActive(true);
                _weaponModel.Reset();
                _weaponModel = null;
                _weaponHandler = null;
                _handIkHandler.SetIkWeapon(null);
            }

            return canDiscard;
        }

        public override void TryToUse()
        {
            if (_weaponHandler != null)
            {
                if (_weaponHandler.TryToUse())
                {
                    AudioManager.Instance.Play(_weaponModel.Details.ShootAudioEffect);
                }
            }
        }

        public override WeaponDetails GetWeapon()
        {
            // ReSharper disable once MergeConditionalExpression
            return _weaponModel == null ? null : _weaponModel.Details;
        }

        private void Equip(WeaponModel weapon)
        {
            SetSelectedGunState(false);
            _weaponModel = weapon;
            _player.Hit.Add(weapon.Details.HitPoints);
            SetSelectedGunState(true);

            _handIkHandler.SetIkWeapon(_weaponHandler);
        }

        private void SetSelectedGunState(bool state)
        {
            if (_weaponModel == null)
                return;

            Transform currentGunTransform = transform.GetChild(_weaponModel.Details.WeaponId);
            if (state)
            {
                _weaponHandler = currentGunTransform.GetComponent<WeaponHandler>();
            }
            else
            {
                _weaponHandler = null;
            }
            //TODO Update animator parameters values
            currentGunTransform.gameObject.SetActive(state);
        }
    }
}
