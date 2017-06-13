using UnityEngine;

namespace ZonkaZombies.Spawn
{
    public class SpawnPointsZombiesComponent : MonoBehaviour
    {
        private float[] _allSpawnDelays;

        public float[] AllSpawnDelays
        {
            get
            {
                return _allSpawnDelays;
            }
        }

        private void Awake()
        {
            if (_allSpawnDelays == null)
            {
                _allSpawnDelays = new[]
                {
                        BasicSpawnDelay, CrawlerSpawnDelay,
                        FasterSpawnDelay, ExplosiveSpawnDelay,
                        ArmorSpawnDelay
                    };
            }
        }

        [Header("Basic Zombie")]
        public bool UseBasicFieldOfView;
        public Transform[] SpawnPointsBasic;
        public GameObject PrefabBasic;
        public float BasicSpawnDelay = 3f;
        
        [Header("Crawler Zombie")]
        public bool UseCrawlerFieldOfView;
        public Transform[] SpawnPointsCrawler;
        public GameObject PrefabCrawler;
        public float CrawlerSpawnDelay = 3f;

        [Header("Explosive Zombie")]
        public bool UseExplosiveFieldOfView;
        public Transform[] SpawnPointsExplosive;
        public GameObject PrefabExplosive;
        public float ExplosiveSpawnDelay = 20f;

        [Header("Faster Zombie")]
        public bool UseFasterFieldOfView;
        public Transform[] SpawnPointsFaster;
        public GameObject PrefabFaster;
        public float FasterSpawnDelay = 10f;

        [Header("Armor Zombie")]
        public bool UseArmorFieldOfView;
        public Transform[] SpawnPointsArmor;
        public GameObject PrefabArmor;
        public float ArmorSpawnDelay = 20f;
    }
}