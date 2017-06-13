using UnityEngine;

namespace ZonkaZombies.Characters.Enemy.Data
{
    [CreateAssetMenu(fileName = "enemyDetails", menuName = "ZonkaZombies/Enemy/EnemyDetails")]
    public class EnemyDetails : ScriptableObject
    {
        public AudioClip AnyMurmur
        {
            get { return MurmursClip[Random.Range(0, MurmursClip.Length)]; }
        }

        public AudioClip[] MurmursClip;

        public AudioClip GotHitClip;
    }
}