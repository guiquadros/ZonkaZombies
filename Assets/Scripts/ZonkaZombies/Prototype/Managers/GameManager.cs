using System.Linq;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Prototype.Scenery.Interaction;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Managers
{
    public enum GameModeType
    {
        Singleplayer, Multiplayer
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private InputReader _inputReader;

        protected override void Awake()
        {
            base.Awake();
            _inputReader = InputFactory.Create(InputType.Controller1);
        }

        private EntityManager _entityManager;
        /// <summary>
        /// Holds the ramaining quantity of collectables into the current scene.
        /// </summary>
        private int _toDoMissionsCount;

        public GameModeType GameMode;

        private void Start()
        {
            _entityManager = EntityManager.Instance;
        }

        private void Update()
        {
            if (_inputReader.StartDown())
            {
                //TODO: fix: is entering many times here
                SceneController.Instance.LoadNextScene();
            }
        }

        /// <summary>
        /// Call this method to update the references os the EntityManager and GameManager classes.
        /// </summary>
        internal void UpdateReferences()
        {
            _entityManager.UpdateReferences();

            foreach (var enemy in _entityManager.Enemies)
            {
                enemy.OnDead += OnEnemyDead;
            }

            foreach (var player in _entityManager.Players)
            {
                player.OnDead += OnPlayerDead;
            }

            InteractableBase[] interactablesInScene = FindObjectsOfType<InteractableBase>();
            _toDoMissionsCount = interactablesInScene.Length;
            foreach (InteractableBase interactable in interactablesInScene)
            {
                interactable.OnInteract += OnGetInteractable;
            }
        }

        private void OnEnemyDead(Character character)
        {
            // ReSharper disable once DelegateSubtraction
            character.OnDead -= OnEnemyDead;

            // Don't destroy the enemy, just disable It
            character.gameObject.SetActive(false);

            if (_entityManager.AreAllEnemiesDead())
            {
                SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
            }
        }

        private void OnPlayerDead(Character character)
        {
            SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME);
        }

        private void OnGetInteractable(InteractableBase interactable)
        {
            _toDoMissionsCount--;

            if (_toDoMissionsCount <= 0)
            {
                SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
            }
        }
    }
}