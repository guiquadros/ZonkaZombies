using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Characters.Enemy;
using ZonkaZombies.Characters.Player;
using ZonkaZombies.Util;

namespace ZonkaZombies.Managers
{
    public class EntityManager : SingletonMonoBehaviour<EntityManager>
    {
        [HideInInspector]
        public List<Player> Players;
        [HideInInspector]
        public List<Enemy> Enemies;

        public bool AreAllEnemiesDead()
        {
            return !Enemies.Any(e => e.IsAlive);
        }

        public Player GetNearestPlayer(Enemy enemy)
        {
            float minDistanceFound = float.MaxValue;
            Player result = null;
            foreach (Player player in Players)
            {
                if (player == null)
                    continue;

                float playerDistance = Vector3.Distance(enemy.transform.position, player.transform.position);
                if (result == null || playerDistance < minDistanceFound)
                {
                    minDistanceFound = playerDistance;
                    result = player;
                }
            }
            return result;
        }

        /// <summary>
        /// This method is used by the GameManager class when It's necessary to update the references inside EntityManager class.
        /// </summary>
        internal void UpdateReferences()
        {
            Players = FindObjectsOfType<Player>().ToList();
            Enemies = FindObjectsOfType<Enemy>().ToList();
        }
    }
}