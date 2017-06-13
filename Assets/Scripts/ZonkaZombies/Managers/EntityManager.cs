using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Characters.Enemy.EnemyIA;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.Util;

namespace ZonkaZombies.Managers
{
    public class EntityManager : IDisposable
    {
        private static EntityManager _instance;
        public static EntityManager Instance
        {
            get { return _instance ?? (_instance = new EntityManager()); }
        }

        public List<Player> Players = new List<Player>(2);
        public List<GenericEnemy> Enemies = new List<GenericEnemy>(10);

        public void Initialize()
        {
            MessageRouter.AddListener<OnPlayerHasBornMessage>(OnPlayerHasBornCallback);
            MessageRouter.AddListener<OnEnemyHasBornMessage>(OnEnemyHasBornCallback);
            MessageRouter.AddListener<OnEnemyDeadMessage>(OnEnemyDeadCallback);
            MessageRouter.AddListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);            
        }

        public void Dispose()
        {
            MessageRouter.RemoveListener<OnPlayerHasBornMessage>(OnPlayerHasBornCallback);
            MessageRouter.RemoveListener<OnEnemyHasBornMessage>(OnEnemyHasBornCallback);
            MessageRouter.RemoveListener<OnEnemyDeadMessage>(OnEnemyDeadCallback);
            MessageRouter.RemoveListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
        }

        public Player GetNearestPlayer(Transform pos)
        {
            float minDistanceFound = float.MaxValue;
            Player result = null;
            foreach (Player player in Players)
            {
                if (player == null || !player.IsAlive)
                {
                    continue;
                }

                float playerDistance = Vector3.Distance(pos.transform.position, player.transform.position);
                if (result == null || playerDistance < minDistanceFound)
                {
                    minDistanceFound = playerDistance;
                    result = player;
                }
            }
            return result;
        }

        public bool AreAllEnemiesDead()
        {
            return !Enemies.Any(e => e.IsAlive);
        }

#region CALLBACKS

        private void OnEnemyHasBornCallback(OnEnemyHasBornMessage message)
        {
            if (message.Enemy == null)
            {
                Debug.LogError("Enemy must not be null!");
            }

            Enemies.Add(message.Enemy);
        }

        private void OnPlayerHasBornCallback(OnPlayerHasBornMessage message)
        {

            if (message.Player == null)
            {
                Debug.LogError("Player must not be null!");
            }

            Players.Add(message.Player);
        }

        private void OnEnemyDeadCallback(OnEnemyDeadMessage message)
        {
            if (message.Enemy == null)
            {
                Debug.LogError("Enemy must not be null!");
            }

            Enemies.Remove(message.Enemy);
        }

        private void OnPlayerDeadCallback(OnPlayerDeadMessage message)
        {
            if (message.Player == null)
            {
                Debug.LogError("Player must not be null!");
            }

            Players.Remove(message.Player);

            if (Players.Count <= 0)
            {
                MessageRouter.SendMessage(new OnAllPlayersAreDead());
            }
        }

#endregion
    }
}