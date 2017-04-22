﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZonkaZombies.Prototype.Characters.Enemy;
using ZonkaZombies.Prototype.Characters.PlayerCharacter;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Managers
{
    public class EntityManager : SingletonMonoBehaviour<EntityManager>
    {
        [HideInInspector]
        public List<Player> Players;
        [HideInInspector]
        public List<Enemy> Enemies;

        private void Start()
        {
            Players = FindObjectsOfType<Player>().ToList();
            Enemies = FindObjectsOfType<Enemy>().ToList();
        }

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
                float playerDistance = Vector3.Distance(enemy.transform.position, player.transform.position);
                if (result == null || playerDistance < minDistanceFound)
                {
                    minDistanceFound = playerDistance;
                    result = player;
                }
            }
            return result;
        }
    }
}