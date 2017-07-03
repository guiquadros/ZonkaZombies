using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Characters;
using ZonkaZombies.Characters.Enemy.EnemyIA;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Characters.Player.Weapon;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;
using ZonkaZombies.Scenery.Interaction;
using ZonkaZombies.UI;
using ZonkaZombies.UI.Data;
using ZonkaZombies.UI.Dialogues;
using ZonkaZombies.Util;

namespace ZonkaZombies.Managers
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public bool MissionCompleted { get; set; }
        
        /// <summary>
        /// Holds the ramaining quantity of collectables into the current scene.
        /// </summary>
        private int _toDoMissionsCount;

        private void OnEnable()
        {
            //Desubscribe to player and enemy callbacks
            MessageRouter.AddListener<OnPlayerHasBornMessage>(OnPlayerHasBornCallback);
            MessageRouter.AddListener<OnEnemyHasBornMessage>(OnEnemyHasBornCallback);
            MessageRouter.AddListener<OnEnemyDeadMessage>(OnEnemyDeadCallback);

            //Subscribe to gameplay callbacks
            MessageRouter.AddListener<OnAllPlayersAreDead>(OnAllPlayersAreDeadCallback);

            EntityManager.Instance.Initialize();
        }

        private void OnDisable()
        {
            //Desubscribe to player and enemy callbacks
            MessageRouter.RemoveListener<OnPlayerDeadMessage>(OnPlayerDeadCallback);
            MessageRouter.RemoveListener<OnEnemyHasBornMessage>(OnEnemyHasBornCallback);
            MessageRouter.RemoveListener<OnEnemyDeadMessage>(OnEnemyDeadCallback);

            //Desubscribe to gameplay callbacks
            MessageRouter.RemoveListener<OnAllPlayersAreDead>(OnAllPlayersAreDeadCallback);

            EntityManager.Instance.Dispose();
        }

        private void Start()
        {
            SceneController.Instance.AfterSceneLoad += OnAfterSceneLoad;
            FindInteractablesReference();
        }

        private void FindInteractablesReference()
        {
            CollectableInteractable[] interactablesInScene = FindObjectsOfType<CollectableInteractable>();
            foreach (CollectableInteractable interactable in interactablesInScene)
            {
                interactable.OnInteract += OnGetInteractable;
            }

            _toDoMissionsCount = interactablesInScene.Count(i => i.Type == InteractableType.Chocolate);
            GameUIManager.Instance.UpdatePlayerMissionCountText(_toDoMissionsCount);
        }

        private void OnAfterSceneLoad()
        {
            FindInteractablesReference();
        }

#region GAMEPLAY LOGIC - METHODS

        private void OnCharacterDamaged(int current, int damage, Character character)
        {
            if (character.IsAlive)
            {
                return;
            }

            character.Health.RemoveListener(OnCharacterDamaged);

            Player player = character as Player;
            if (player != null)
            {
                AudioManager.Instance.Play(player.PlayerDetails.DyingClip);
                Destroy(player.gameObject);
                MessageRouter.SendMessage(new OnPlayerDeadMessage(player));
            }

            GenericEnemy enemy = character as GenericEnemy;
            if (enemy != null)
            {
                MessageRouter.SendMessage(new OnEnemyDeadMessage(enemy));
            }
        }

        private void OnGetInteractable(InteractableBase interactable, Player player, params object[] args)
        {
            if (interactable is CollectableInteractable)
            {
                CollectableInteractable collectableInteractable = (CollectableInteractable) interactable;

                switch (collectableInteractable.Type)
                {
                    case InteractableType.Chocolate:
                        GameUIManager.Instance.UpdatePlayerMissionCountText(--_toDoMissionsCount);

                        if (_toDoMissionsCount <= 0)
                        {
                            MissionCompleted = true;
                            SceneController.Instance.FadeAndLoadScene(GameScenes.DIALOGUE_SCIENTIST);

                            //TODO: performance issues
                            //EntityManager.Instance.Enemies.ForEach(e => e.UseFieldOfView = false);
                            //MessageRouter.SendMessage(new ForceEnemyPursuitMode());
                        }
                        break;
                    case InteractableType.Weapon:
                        WeaponModel weaponModel = (WeaponModel) args[0];
                        player.SelectWeapon(weaponModel);
                        break;
                }
            }
        }

#endregion

#region MESSAGE ROUTER - CALLBACKS

        public void OnEnemyHasBornCallback(OnEnemyHasBornMessage message)
        {
            message.Enemy.Health.AddListener(OnCharacterDamaged);
        }

        private void OnPlayerHasBornCallback(OnPlayerHasBornMessage message)
        {
            message.Player.Health.AddListener(OnCharacterDamaged);
        }

        public void OnEnemyDeadCallback(OnEnemyDeadMessage message)
        {
            message.Enemy.Health.RemoveListener(OnCharacterDamaged);
        }

        private void OnPlayerDeadCallback(OnPlayerDeadMessage message)
        {
            message.Player.Health.RemoveListener(OnCharacterDamaged);
        }

        private void OnAllPlayersAreDeadCallback(OnAllPlayersAreDead message)
        {
            GameOver();
        }

#endregion

        public void GameOver()
        {
            EntityManager.Instance.Enemies.Clear();
            SceneController.Instance.FadeAndLoadScene(GameScenes.GAME_OVER_SCENE);
        }
    }
}