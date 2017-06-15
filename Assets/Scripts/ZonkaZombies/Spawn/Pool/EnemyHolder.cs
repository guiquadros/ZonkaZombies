using UnityEngine;

namespace ZonkaZombies.Spawn.Pool
{
    [CreateAssetMenu(fileName = "newEnemyHolder", menuName = "ZonkaZombies/Enemy/EnemyHolder")]
    public class EnemyHolder : ScriptableObject
    {
        [Header("Prefabs Instances")]
        public GameObject Basic;
        public GameObject Crawler;
        public GameObject Faster;
        public GameObject Explosive;
        public GameObject Armor;

        public GameObject Instantiate(EEnemyType type)
        {
            switch (type)
            {
                case EEnemyType.Basic:
                    return Instantiate(Basic);
                case EEnemyType.Crawler:
                    return Instantiate(Crawler);
                case EEnemyType.Faster:
                    return Instantiate(Faster);
                case EEnemyType.Explosive:
                    return Instantiate(Explosive);
                case EEnemyType.Armor:
                    return Instantiate(Armor);
                default:
                    return null;
            }
        }
    }
}