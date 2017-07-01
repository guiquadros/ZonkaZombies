using System.Linq;
using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;
using ZonkaZombies.Managers;
using ZonkaZombies.Util;
using Random = UnityEngine.Random;

namespace ZonkaZombies.Spawn
{
    [RequireComponent(typeof(SpawnPointsZombiesComponent))]
    [RequireComponent(typeof(Collider))]
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private int _maxEnemiesSpawn = 100;

        private float _time;
        private SpawnPointsZombiesComponent _spawnDataZombies;
        private bool _stopSpawn;
        private readonly float _tolerance = 0.001f;
        private int _intTime;

        private void Awake()
        {
            _spawnDataZombies = GetComponent<SpawnPointsZombiesComponent>();
        }

        private void Update()
        {
#if !DISABLE_ENEMY_SPAWN
            _time += Time.deltaTime;

            if (_time > _spawnDataZombies.AllSpawnDelays.Max())
            {
                _time = 0f;
            }
#endif
        }

        private void SpawnEnemiesInRandomSpawnPoints()
        {
            int newIntTime = (int) _time;

            //only counts integer values (seconds)
            if (newIntTime == _intTime)
            {
                return;
            }

            _intTime = newIntTime;

            if (_intTime % _spawnDataZombies.BasicSpawnDelay < _tolerance)
            {
                SpawnEnemyInRandomSpawnPoint(_spawnDataZombies.PrefabBasic, _spawnDataZombies.SpawnPointsBasic, _spawnDataZombies.UseBasicFieldOfView);
            }

            if (_intTime % _spawnDataZombies.CrawlerSpawnDelay < _tolerance)
            {
                SpawnEnemyInRandomSpawnPoint(_spawnDataZombies.PrefabCrawler, _spawnDataZombies.SpawnPointsCrawler, _spawnDataZombies.UseCrawlerFieldOfView);
            }

            if (_intTime % _spawnDataZombies.FasterSpawnDelay < _tolerance)
            {
                SpawnEnemyInRandomSpawnPoint(_spawnDataZombies.PrefabFaster, _spawnDataZombies.SpawnPointsFaster, _spawnDataZombies.UseFasterFieldOfView);
            }

            if (_intTime % _spawnDataZombies.ExplosiveSpawnDelay < _tolerance)
            {
                SpawnEnemyInRandomSpawnPoint(_spawnDataZombies.PrefabExplosive, _spawnDataZombies.SpawnPointsExplosive, _spawnDataZombies.UseExplosiveFieldOfView);
            }

            if (_intTime % _spawnDataZombies.ArmorSpawnDelay < _tolerance)
            {
                SpawnEnemyInRandomSpawnPoint(_spawnDataZombies.PrefabArmor, _spawnDataZombies.SpawnPointsArmor, _spawnDataZombies.UseArmorFieldOfView);
            }
        }

        private void SpawnEnemyInRandomSpawnPoint(GameObject enemyPrefab, Transform[] spawnPoints, bool useFieldOfView)
        {
            if (spawnPoints.Length == 0 || enemyPrefab == null) return;

            Transform spawnPoint = GetRandomSpawnPoint(spawnPoints);
            
            //TODO: verify if there is no other object (player or enemy) in this position before spawn the enemy.
            //var totalCollided = Physics.OverlapSphere(spawnPoint.position, 3000f, LayerConstants.ENEMY_LAYER).Length;
            //totalCollided += Physics.OverlapSphere(spawnPoint.position, 3000f, LayerConstants.PLAYER_CHARACTER_LAYER).Length;
            //Debug.Log(totalCollided);

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            GenericEnemy genericEnemyComponent = enemyInstance.GetComponent<GenericEnemy>();

            if (genericEnemyComponent != null)
            {
                genericEnemyComponent.UseFieldOfView = useFieldOfView;
            }
        }

        private Transform GetRandomSpawnPoint(Transform[] spawnPoints)
        {
            int i = Random.Range(0, spawnPoints.Length - 1);
            return spawnPoints[i];
        }

        //do spawn only if the playe enters on the collision
        private void OnTriggerStay(Collider other)
        {
            if (_stopSpawn || _time <= 0f || EntityManager.Instance.Enemies.Count >= _maxEnemiesSpawn) return;

            if (other.CompareTag(TagConstants.PLAYER))
            {
                 SpawnEnemiesInRandomSpawnPoints();
            }
        }

        public void StopSpawn()
        {
            _stopSpawn = true;
        }
    }
}
