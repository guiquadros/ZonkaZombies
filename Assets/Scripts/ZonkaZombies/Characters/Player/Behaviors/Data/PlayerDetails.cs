using UnityEngine;

namespace ZonkaZombies.Characters.Player.Behaviors.Data
{
    [CreateAssetMenu(fileName = "playerDetails", menuName = "ZonkaZombies/Player/PlayerDetails")]
    public class PlayerDetails : ScriptableObject
    {
        public AudioClip AnyTakeDamageClip
        {
            get { return TakeDamageClips[Random.Range(0, TakeDamageClips.Length)]; }
        }

        public AudioClip[] TakeDamageClips;

        //public AudioClip AnyFootstepClip
        //{
        //    get { return FootstepClips[Random.Range(0, FootstepClips.Length)]; }
        //}

        //public AudioClip[] FootstepClips;

        public AudioClip DyingClip;
    }
}