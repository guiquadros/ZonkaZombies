using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Characters;
using ZonkaZombies.Input;
using ZonkaZombies.Scenery.Interaction;
using ZonkaZombies.Util;

namespace ZonkaZombies.Managers
{
    public enum GameModeType
    {
        Singleplayer, Multiplayer
    }
    
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private InputReader _inputReader;
        private EntityManager _entityManager;
        private SceneController _sceneController;
        
        /// <summary>
        /// Holds the ramaining quantity of collectables into the current scene.
        /// </summary>
        private int _toDoMissionsCount;

        public GameModeType GameMode;

        protected override void Awake()
        {
            base.Awake();
            _inputReader = InputFactory.Create(InputType.Controller1);
        }

        private void Start()
        {
            _entityManager = EntityManager.Instance;
            _sceneController = Managers.SceneController.Instance;
        }

        private void Update()
        {
            if (_inputReader.StartDown() || UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Start button down. Loading next scene.");
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

            CollectableInteractable[] interactablesInScene = FindObjectsOfType<CollectableInteractable>();
            _toDoMissionsCount = interactablesInScene.Length;
            foreach (CollectableInteractable interactable in interactablesInScene)
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