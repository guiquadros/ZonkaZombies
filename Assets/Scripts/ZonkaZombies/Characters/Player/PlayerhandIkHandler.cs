using UnityEngine;
using ZonkaZombies.Characters.Player.Weapon;

namespace ZonkaZombies.Characters.Player
{
    public class PlayerhandIkHandler : MonoBehaviour
    {
        private WeaponHandler _weaponHandler = null;

        public Animator Animator;

        private void OnAnimatorIK(int layerIndex)
        {
            if (_weaponHandler == null)
            {
                Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
            else
            {
                Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                Animator.SetIKPosition(AvatarIKGoal.LeftHand, _weaponHandler.IkTransforms[EHand.Left.Value()].position);
                Animator.SetIKRotation(AvatarIKGoal.LeftHand, _weaponHandler.IkTransforms[EHand.Left.Value()].rotation);
                Animator.SetIKPosition(AvatarIKGoal.RightHand, _weaponHandler.IkTransforms[EHand.Right.Value()].position);
                Animator.SetIKRotation(AvatarIKGoal.RightHand, _weaponHandler.IkTransforms[EHand.Right.Value()].rotation);
            }
        }

        public void SetIkWeapon(WeaponHandler weapon)
        {
            _weaponHandler = weapon;
        }
    }
}