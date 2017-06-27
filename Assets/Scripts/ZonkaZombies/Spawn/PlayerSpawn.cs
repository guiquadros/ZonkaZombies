using UnityEngine;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Managers;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;

namespace ZonkaZombies.Spawn
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] 
        private Transform _spawnPointPlayer1, _spawnPointPlayer2;

        private void Start()
        {
            for (var i = 0; i < EntityManager.Instance.Players.Count; i++)
            {
                SpawnPlayer(EntityManager.Instance.Players[i], EntityManager.Instance.Players[i].IsFirstPlayer ? _spawnPointPlayer1 : _spawnPointPlayer2);
            }
        }

        private void SpawnPlayer(Player player, Transform playerTransform)
        {
            player.transform.parent = playerTransform;
            player.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
            player.transform.rotation = playerTransform.rotation;

            MessageRouter.SendMessage(new OnPlayerSpawnMessage(player));
        }
    }
}
