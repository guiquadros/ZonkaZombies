using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;

namespace ZonkaZombies.Spawn.Pool
{
    public class EnemyPoolManager : MonoBehaviour
    {
        private static EnemyPoolManager _instance;
        private static EnemyPoolManager Instance
        {
            get { return _instance ?? (_instance = FindObjectOfType<EnemyPoolManager>()); }
        }

        private readonly Dictionary<EEnemyType, List<GenericEnemy>> _enemiesInstances = new Dictionary<EEnemyType, List<GenericEnemy>>();
        private readonly Dictionary<EEnemyType, GameObject> _enemiesParents = new Dictionary<EEnemyType, GameObject>();

        [Header("Setup"), Range(0, 100)]
        public int PreInstantiatedCount = 20;

        [SerializeField]
        private EnemyHolder _enemyHolder;

        private void Awake()
        {
            foreach (EEnemyType enemyType in Enum.GetValues(typeof(EEnemyType)))
            {
                GameObject parent = new GameObject(string.Concat(enemyType.ToString(), " Pool"));
                parent.transform.SetParent(gameObject.transform);
                _enemiesParents.Add(enemyType, parent);

                _enemiesInstances.Add(enemyType, new List<GenericEnemy>());

                for (int i = 0; i < PreInstantiatedCount; i++)
                {
                    Instantiate(enemyType);
                }
            }
        }

        private GenericEnemy Instantiate(EEnemyType type)
        {
            GameObject instance = _enemyHolder.Instantiate(type);

            // Always set the enemy inactive when instantiated
            instance.SetActive(false);

            instance.transform.SetParent(_enemiesParents[type].transform);

            GenericEnemy genericEnemy = instance.GetComponent<GenericEnemy>();

            _enemiesInstances[type].Add(genericEnemy);

            return genericEnemy;
        }

        public static GenericEnemy Pop(EEnemyType type)
        {
            GenericEnemy enemyInstance = Instance._enemiesInstances[type].FirstOrDefault(e => !e.gameObject.activeSelf);

            if (enemyInstance == null)
            {
                enemyInstance = Instance.Instantiate(type);
            }

            return enemyInstance;
        }
    }
}