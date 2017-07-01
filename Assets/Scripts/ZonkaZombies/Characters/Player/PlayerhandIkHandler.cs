using UnityEngine;
using ZonkaZombies.Characters.Player.Weapon;

namespace ZonkaZombies.Characters.Player
{
    public class PlayerhandIkHandler : MonoBehaviour
    {
        private WeaponHandler _weaponHandler = null;

        public Animator Animator;

        private float _ikWeight    = 0;
        private float _curIkWeight = 0;

        private void OnAnimatorIK(int layerIndex)
        {
            _ikWeight = _weaponHandler == null ? 0 : 1;

            _curIkWeight = Mathf.MoveTowards(_curIkWeight, _ikWeight, Time.deltaTime);

            Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _curIkWeight);
            Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _curIkWeight);
            Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _curIkWeight);
            Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _curIkWeight);
            Animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, _curIkWeight);
            Animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, _curIkWeight);

            if (_weaponHandler == null)
            {
                return;
            }

            Animator.SetIKPosition(AvatarIKGoal.LeftHand,
                _weaponHandler.IkTransforms[EikPosition.LeftHand.Value()].position);
            Animator.SetIKRotation(AvatarIKGoal.LeftHand,
                _weaponHandler.IkTransforms[EikPosition.LeftHand.Value()].rotation);
            Animator.SetIKPosition(AvatarIKGoal.RightHand,
                _weaponHandler.IkTransforms[EikPosition.RightHand.Value()].position);
            Animator.SetIKRotation(AvatarIKGoal.RightHand,
                _weaponHandler.IkTransforms[EikPosition.RightHand.Value()].rotation);
            Animator.SetIKHintPosition(AvatarIKHint.LeftElbow,
                _weaponHandler.IkTransforms[EikPosition.LeftElbow.Value()].position);
            Animator.SetIKHintPosition(AvatarIKHint.RightElbow,
                _weaponHandler.IkTransforms[EikPosition.RightElbow.Value()].position);
        }

        public void SetIkWeapon(WeaponHandler weapon)
        {
            _weaponHandler = weapon;
        }
    }
}