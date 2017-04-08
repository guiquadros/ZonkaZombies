using UnityEngine;

namespace ZonkaZombies.Prototype.PlayerCharacter
{
    public class ArmsDamagerHandler : MonoBehaviour
    {
        [SerializeField] private Collider _leftArmDamager;

#region LEFT ARM
        public void EnableLeftArmDamager()
        {
            _leftArmDamager.enabled = true;
        }

        public void DisableLeftArmDamager()
        {
            _leftArmDamager.enabled = false;
        }
#endregion
    }
}
